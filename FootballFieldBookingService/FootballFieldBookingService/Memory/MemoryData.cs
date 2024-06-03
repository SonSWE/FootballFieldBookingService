using CommonLib;
using ObjectInfo;
using System.Data;
using FootballFieldBookingService.Memory;

namespace FootballFieldBookingService
{
    public class MemoryData
    {
        public static void LoadMemory()
        {
            try
            {
                AllCode_Memory.Reload();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
        }
    }
}
