using System;
using System.IO;
using System.Linq;
using Ajf.NugetWatcher.Settings;
using JCI.ITC.Nuget.Logging.Settings;
using JCI.ITC.Nuget.TopShelf;
using Serilog;

namespace Ajf.NugetWatcher
{
    public class WorkerDirWatcher : BaseWorker, IDisposable
    {
        private readonly IMailSenderService _mailSenderService;
        private readonly ILoggingSettings _loggingSettings;

        public WorkerDirWatcher(IMailSenderService mailSenderService, ILoggingSettings loggingSettings)
        {
            _mailSenderService = mailSenderService;
            _loggingSettings = loggingSettings;
        }
        private FileSystemWatcher _fileSystemWatcher;

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
            var httpStatusCode = _mailSenderService
                .SendMail("[NugetWatcher] Nuget Change", "", "<b>hello</b>",
                "andersjuulsfirma@gmail.com;Anders", new[] {"andersjuulsfirma@gmail.com;Anders"})
                ;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var path = e.FullPath;
            var enumerateDirectories = Directory.EnumerateDirectories(path);

            var datedDirs =
                enumerateDirectories.Select(x => new DatedDir {Path = x, Ts = Directory.GetLastWriteTime(x)}).OrderByDescending(xx=>xx.Ts);

            var latest = datedDirs.FirstOrDefault();
            if(latest==null)return;

            var httpStatusCode = _mailSenderService
                .SendMail($"[NugetWatcher]  {latest.Path} {latest.Ts}", "", 
                    $"<b>This was send from {_loggingSettings.SuiteName}.{_loggingSettings.ComponentName}, {_loggingSettings.Environment}, {_loggingSettings.ReleaseNumber}</b>",
                    "andersjuulsfirma@gmail.com;Anders", new[] { "andersjuulsfirma@gmail.com;Anders" })
                ;
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