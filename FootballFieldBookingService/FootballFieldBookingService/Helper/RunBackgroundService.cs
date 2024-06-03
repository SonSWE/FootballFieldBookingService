using CommonLib;
using CommonLib.Helper;
using DataAccess;
using ObjectInfo;
using System.Linq;
using System.Net;

namespace FootballFieldBookingService.Helper
{
    public class RunBackgroundService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(CacheMemoryData, cancellationToken); //Load memory: 5 min


            return Task.CompletedTask;
        }

        public Task CacheMemoryData()
        {
            while (true)
            {
                try
                {
                    MemoryData.LoadMemory();
                }
                catch (Exception ex)
                {
                    Logger.log.Error(ex.ToString());
                }
                Thread.Sleep(5 * 60 * 1000);
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
