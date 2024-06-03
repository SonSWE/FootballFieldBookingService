using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class TimeSlotInfo : MasterDataBase
    {
        public decimal TimeSlotId { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public int Position { get; set; }
        public decimal FootballFieldId { get; set; }
    }
}
