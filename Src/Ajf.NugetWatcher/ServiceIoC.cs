using StructureMap;

namespace Ajf.NugetWatcher
{
    public static class ServiceIoC
    {
        /// <summary>
        /// </summary>
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ServiceRegistry>();
            });
        }
    }
}