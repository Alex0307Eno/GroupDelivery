using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreClosedDate
    {
        public int StoreClosedDateId { get; set; }

        public int StoreId { get; set; }
        // 關店日期（只關心年月日，不關心時間）
        public DateTime ClosedDate { get; set; }
        // 記錄新增的時間，方便未來查詢和管理
        public DateTime CreatedAt { get; set; }
        // 導航屬性，方便從關店日期查詢對應的商店
        public Store Store { get; set; }
    }
}
