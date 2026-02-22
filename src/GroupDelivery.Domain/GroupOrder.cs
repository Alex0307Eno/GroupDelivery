using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GroupDelivery.Domain
{
    // 開團 tbGroupOrder
    [Table("tbGroupOrder")]
    public class GroupOrder
    {
        [Key]
        public int GroupOrderId { get; set; }
        // 關聯：此團單屬於哪個店家
        public int OwnerUserId { get; set; }

        public int StoreId { get; set; }
        // 關聯：此團單由哪個使用者（商家）開立
        public int CreatorUserId { get; set; }
        // 團購目標金額（達成此金額即成團成功）
        public decimal TargetAmount { get; set; }
        // 團購目前金額（由訂單累積而來）
        public decimal CurrentAmount { get; set; }
        // 團購截止時間（超過此時間即結束，無論是否達標）
        public DateTime Deadline { get; set; }
        // 團購狀態
        [Required]
        [StringLength(20)]
        public GroupOrderStatus Status { get; set; } // Pending, Active, Success, Failed
        // 備註（可選）
        [StringLength(500)]
        public string Remark { get; set; }
        // 建立時間
        public DateTime CreatedAt { get; set; }
        
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
        public virtual ICollection<GroupOrderItem> GroupOrderItems { get; set; }
        public virtual User OwnerUser { get; set; }
        public ICollection<Order> Orders { get; set; }



    }
}
