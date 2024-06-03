using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class BillInfo : MasterDataBase
    {
        public decimal BillId { get; set; }
        public DateTime Date_Checkout { get; set; }
        public decimal CustomerId { get; set; }
        public decimal FootballFieldId { get; set; }
    }
}
