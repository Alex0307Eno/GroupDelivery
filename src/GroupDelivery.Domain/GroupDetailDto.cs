using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class GroupDetailDto
    {
        public int GroupId { get; set; }
        /// 團單所屬店家名稱
        public string StoreName { get; set; }
        /// 成團目標金額
        public decimal TargetAmount { get; set; }
        /// 已達成金額
        public decimal CurrentAmount { get; set; }
        /// 成團截止時間
        public DateTime Deadline { get; set; }
        /// 團單所屬店家菜單圖片列表（URL）
        public List<string> MenuImages { get; set; }
    }
}
