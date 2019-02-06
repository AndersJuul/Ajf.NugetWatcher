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

                //c.For<IFluidMediumFactory>().Use<TestFluidMediumFactory>();
                //c.AddType(typeof(IUnitCalculator), typeof(DanfossExpansionValveCalculator));


                //// Get all implementations of IUnitCalculator from the nuget package JCI.ITC.CoreUnits.
                //c.Scan(scan =>
                //{
                //    //scan.AssemblyContainingType<DisplacementMachineCalculator>();
                //    scan.AddAllTypesOf<IUnitCalculator>();

                //    scan.WithDefaultConventions();
                //});
            });
        }
    }
}