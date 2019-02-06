using System;
using System.Collections.Generic;
using JCI.ITC.COMP2.Common.SettingsEnrichers;
using JCI.ITC.Nuget.Logging.SettingsEnrichers;
using Serilog.Events;

namespace Ajf.NugetWatcher.Settings
{
    public class NugetWatcherSettings: INugetWatcherSettings
    {
        public NugetWatcherSettings()
        {
            LoggingSettingsEnricher.Enrich(this);
            NugetWatcherSettingsEnricher.Enrich(this);
            MailSenderSettingsEnricher.Enrich(this);
        }
        public string ReleaseNumber { get; set; }
        public string ComponentName { get; set; }
        public string SuiteName { get; set; }
        public string Environment { get; set; }
        public string LogFileDirectory { get; set; }
        public string FileName { get; set; }
        public string EsLoggingUrl { get; set; }
        public IEnumerable<Uri> EsLoggingUri { get; set; }
        public LogEventLevel LoggingLevel { get; set; }
        public string PathToNuget { get; set; }
        public string SendGridApiKey { get; set; }
    }
}