using System;
using JCI.ITC.Nuget.TopShelf;
using Serilog;

namespace Ajf.NugetWatcher
{
    public class WorkerDirWatcher : BaseWorker, IDisposable
    {
        public void Dispose()
        {
            Stop();
        }

        public override void Start()
        {
            try
            {
                Log.Logger.Information("Starting WorkerDirWatcher");

                Log.Logger.Information("WorkerDirWatcher started");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "During Start", new object[0]);
                throw;
            }
        }


        public override void Stop()
        {
            Log.Logger.Information("Stopping worker");

            Log.Logger.Information("WorkerDirWatcher stopped");
        }
    }
}