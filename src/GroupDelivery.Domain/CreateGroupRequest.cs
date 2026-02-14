using System;
using System.Collections.Generic;

namespace GroupDelivery.Domain
{
    public class CreateGroupRequest
    {
       public int StoreId { get; set; }

        // 成團目標金額
        public decimal TargetAmount { get; set; }

        // 截止時間
        public DateTime Deadline { get; set; }
        // 是否鎖定商店
        public bool IsLockedStore { get; set; }
        // 商店名稱
        public string StoreName { get; set; }
        // 可選商店列表
        public List<Store> AvailableStores { get; set; }


        // 備註
        public string Remark { get; set; }
    }
}
