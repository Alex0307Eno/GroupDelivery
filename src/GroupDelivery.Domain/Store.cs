using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupDelivery.Domain
{
    [Table("tbStore")]
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        // 店家擁有者（對應 User）
        [Required]
        public int OwnerUserId { get; set; }

        // 店名
        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }

        // 聯絡電話
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        // 地址（初始化階段可為空）
        [StringLength(255)]
        public string Address { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // 店家狀態：Draft / Active / Suspended
        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public virtual ICollection<StoreProduct> Products { get; set; }
    }
}
