using System;
using System.IO;
using Ajf.NugetWatcher.Settings;
using JCI.ITC.Nuget.TopShelf;
using Serilog;

namespace Ajf.NugetWatcher
{
    public class WorkerDirWatcher : BaseWorker, IDisposable
    {
        private readonly IMailSenderService _mailSenderService;

        public WorkerDirWatcher(IMailSenderService mailSenderService)
        {
            _mailSenderService = mailSenderService;
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
                .SendMailAsync("[NugetWatcher] Nuget Change", "", "<b>hello</b>",
                "andersjuulsfirma@gmail.com;Anders", new[] {"andersjuulsfirma@gmail.com;Anders"})
                .Result;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var httpStatusCode = _mailSenderService
                .SendMailAsync("[NugetWatcher] Nuget Change", "", "<b>hello</b>",
                    "andersjuulsfirma@gmail.com;Anders", new[] { "andersjuulsfirma@gmail.com;Anders" })
                .Result;
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
}