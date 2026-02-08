using System;

namespace GroupDelivery.Domain
{
    public class CreateGroupRequest
    {
       

        // 成團目標金額
        public decimal TargetAmount { get; set; }

        // 截止時間
        public DateTime Deadline { get; set; }

        // 備註
        public string Remark { get; set; }
    }
}
