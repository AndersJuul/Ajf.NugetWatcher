using JCI.ITC.COMP2.Common.SettingsEnrichers;

namespace Ajf.NugetWatcher.Settings
{
    public static class NugetWatcherSettingsEnricher
    {
        public static void Enrich(INugetWatcherSettings nugetWatcherSettings)
        {
            nugetWatcherSettings.PathToNuget = SettingsEnricher.ValueByKeyString("PathToNuget", true);
            nugetWatcherSettings.NotificationReceivers = SettingsEnricher.ValueByKeyStringArray("NotificationReceivers", true);
        }
    }
}