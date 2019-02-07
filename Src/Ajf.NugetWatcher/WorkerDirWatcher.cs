using System;
using System.IO;
using System.Linq;
using Ajf.NugetWatcher.Settings;
using JCI.ITC.Nuget.TopShelf;
using Newtonsoft.Json;
using Serilog;

namespace Ajf.NugetWatcher
{
    public class WorkerDirWatcher : BaseWorker, IDisposable
    {
        private readonly INugetWatcherSettings _nugetWatcherSettings;
        private readonly IMailSenderService _mailSenderService;
        private FileSystemWatcher _fileSystemWatcher;

        public WorkerDirWatcher(IMailSenderService mailSenderService, INugetWatcherSettings nugetWatcherSettings)
        {
            _mailSenderService = mailSenderService;
            _nugetWatcherSettings = nugetWatcherSettings;
        }

        public void Dispose()
        {
            Stop();
        }

        public override void Start()
        {
            try
            {
                Log.Logger.Information("Starting WorkerDirWatcher");

                var nugetWatcherSettings = new NugetWatcherSettings();
                _fileSystemWatcher = new FileSystemWatcher(nugetWatcherSettings.PathToNuget);
                _fileSystemWatcher.Changed += OnChanged;
                _fileSystemWatcher.Created += OnChanged;
                _fileSystemWatcher.Deleted += OnChanged;
                _fileSystemWatcher.Renamed += OnRenamed;
                _fileSystemWatcher.EnableRaisingEvents = true;

                Log.Logger.Information("WorkerDirWatcher started");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "During Start", new object[0]);
                throw;
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            try
            {
                var httpStatusCode = _mailSenderService
                        .SendMail("[NugetWatcher] Nuget Rename " + JsonConvert.SerializeObject(e), "", "<b>hello</b>",
                            "andersjuulsfirma@gmail.com;Anders", new[] { "andersjuulsfirma@gmail.com;Anders" })
                    ;
            }
            catch (Exception exception)
            {
                Log.Logger.Error(exception, "During on renamed");
                throw;
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                Log.Logger.Information("Change detected: " + JsonConvert.SerializeObject(e));

                var path = e.FullPath;
                var enumerateDirectories = Directory.EnumerateDirectories(path);

                var datedDirs =
                    enumerateDirectories.Select(x => new DatedDir { Path = x, Ts = Directory.GetLastWriteTime(x) })
                        .OrderByDescending(xx => xx.Ts);

                var latest = datedDirs.FirstOrDefault();
                if (latest == null) return;

                var httpStatusCode = _mailSenderService
                        .SendMail($"[NugetWatcher]  {latest.Path} {latest.Ts}", "",
                            $"<b>This was send from {_nugetWatcherSettings.SuiteName}.{_nugetWatcherSettings.ComponentName}, {_nugetWatcherSettings.Environment}, {_nugetWatcherSettings.ReleaseNumber}</b>",
                            "andersjuulsfirma@gmail.com;Anders",_nugetWatcherSettings.NotificationReceivers )
                    ;
            }
            catch (Exception exception)
            {
                Log.Logger.Error(exception,"During on changed");
            }
        }


        public override void Stop()
        {
            Log.Logger.Information("Stopping worker");

            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.Dispose();
                _fileSystemWatcher = null;
            }

            Log.Logger.Information("WorkerDirWatcher stopped");
        }
    }

    internal class DatedDir
    {
        public string Path { get; set; }
        public DateTime Ts { get; set; }
    }
}