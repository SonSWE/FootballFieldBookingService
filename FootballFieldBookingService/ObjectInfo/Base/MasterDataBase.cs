using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class MasterDataBase
    {
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; }
        public string Status_Text { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
    }

    public class MasterDataBaseBusinessResponse
    {
        public long Code { get; set; }
        public string Message { get; set; }
        public string JsonData { get; set; }
        public string PropertyName { get; set; }
        public string Id { get; set; }
    }
}
