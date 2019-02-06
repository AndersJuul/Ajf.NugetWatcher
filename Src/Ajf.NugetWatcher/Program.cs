using System;
using JCI.ITC.Nuget.Logging;
using JCI.ITC.Nuget.TopShelf;
using Serilog;

namespace Ajf.NugetWatcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = StandardLoggerConfigurator
                .GetLoggerConfig().MinimumLevel
                .Debug()
                .CreateLogger();

            Log.Logger.Information("Starting Service with args: " + string.Join("|",args));

            using (var container = ServiceIoC.Initialize())
            {
                Log.Logger.Debug("WhatDoIHave" + "\n" + container.WhatDoIHave());

                Log.Logger.Information("Container created.");

                using (var wrapper = new TopshelfWrapper<WorkerDirWatcher>(
                    () =>{},
                    s =>
                    {
                        s.ConstructUsing(name =>
                        {
                            try
                            {
                                var instance = container.GetInstance<WorkerDirWatcher>();
                                Log.Logger.Debug("I have instance of WorkerDirWatcher!");
                                return instance;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                        });
                    }))
                {
                    try
                    {
                        wrapper.Run();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
    }
}