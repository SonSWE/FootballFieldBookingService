using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class FootballFieldInfo : MasterDataBase
    {
        public decimal FootballFieldId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
