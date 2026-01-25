namespace GroupDelivery.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // 店家 tbStore
    [Table("tbStore")]
    public class Store
    {
        [Key]
        public int StoreId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public virtual ICollection<StoreProduct> Products { get; set; }
    }

    // 商品 tbStoreProduct
    [Table("tbStoreProduct")]
    public class StoreProduct
    {
        [Key]
        public int ProductId { get; set; }
        
        public int StoreId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
    }

    // 使用者 tbUser
    [Table("tbUser")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
        
        [StringLength(20)]
        public string Phone { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }

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

    // 參團明細 tbGroupOrderItem
    [Table("tbGroupOrderItem")]
    public class GroupOrderItem
    {
        [Key]
        public int GroupOrderItemId { get; set; }
        
        public int GroupOrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubtotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("GroupOrderId")]
        public virtual GroupOrder GroupOrder { get; set; }
    }
}
