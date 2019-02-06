using StructureMap;

namespace Ajf.NugetWatcher
{
    public class ServiceRegistry : Registry
    {
        public ServiceRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}