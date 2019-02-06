using Ajf.NugetWatcher.Settings;
using JCI.ITC.COMP2.Common.Settings;
using StructureMap;

namespace Ajf.NugetWatcher
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            var nugetWatcherSettings = new NugetWatcherSettings();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();

            });
            For<IMailSenderService>().Use<MailSenderServiceSendGrid>();
            For<IMailSenderSettings>().Use(nugetWatcherSettings);
        }
    }
}