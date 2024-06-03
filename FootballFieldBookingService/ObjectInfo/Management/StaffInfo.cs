using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class StaffInfo : MasterDataBase
    {
        public decimal StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string StaffPosition { get; set; } = string.Empty;
    }
}
