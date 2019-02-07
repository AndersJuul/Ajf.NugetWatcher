using JCI.ITC.COMP2.Common.Settings;
using JCI.ITC.Nuget.Logging.Settings;

namespace Ajf.NugetWatcher.Settings
{
    public interface INugetWatcherSettings:ILoggingSettings,IMailSenderSettings
    {
        string PathToNuget { get; set; }
        string[] NotificationReceivers { get; set; }
    }
}