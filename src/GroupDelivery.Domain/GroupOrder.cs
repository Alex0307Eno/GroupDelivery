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

        public int StoreId { get; set; }
        public int CreatorUserId { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // Pending, Active, Success, Failed

        [StringLength(500)]
        public string Remark { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }

        [ForeignKey("CreatorUserId")]
        public virtual User Creator { get; set; }
    }
}
