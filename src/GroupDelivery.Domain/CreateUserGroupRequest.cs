using System;
using System.ComponentModel.DataAnnotations;

namespace GroupDelivery.Domain
{
    public class CreateUserGroupRequest
    {
        [Required]
        public int StoreId { get; set; }

        // 成團目標金額
        [Required]
        [Range(1, 1000000)]
        public decimal TargetAmount { get; set; }

        // 截止時間
        [Required]
        public DateTime Deadline { get; set; }

        // 備註
        [StringLength(500)]
        public string Remark { get; set; }
    }
}