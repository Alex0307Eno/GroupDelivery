using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreSummaryDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string CoverImageUrl { get; set; }

        // 之後要算距離會用到
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class GroupSummaryDto
    {
        public int GroupOrderId { get; set; }

        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }

        public DateTime Deadline { get; set; }

        public string Remark { get; set; }

        public StoreSummaryDto Store { get; set; }

        // 首頁顯示用：距離（公里）
        public double Distance { get; set; }
    }
}
