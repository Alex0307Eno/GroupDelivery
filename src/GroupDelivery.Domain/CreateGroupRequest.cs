using System;

namespace GroupDelivery.Domain
{
    public class CreateGroupRequest
    {
        // 開團的店家
        public int StoreId { get; set; }

        // 開團者（之後可以從登入者自動帶）
        public int CreatorUserId { get; set; }

        // 成團目標金額
        public decimal TargetAmount { get; set; }

        // 截止時間
        public DateTime Deadline { get; set; }

        // 外送距離（公里）
        public int DeliveryRange { get; set; }

        // 備註
        public string Remark { get; set; }
    }
}
