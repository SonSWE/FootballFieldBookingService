using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class FieldBookingInfo : MasterDataBase
    {
        public decimal FieldBookingId { get; set; }
        public decimal TimeSlotsId { get; set; }
        public decimal CustomerId { get; set; }
        public DateTime Date { get; set; }
    }
}
