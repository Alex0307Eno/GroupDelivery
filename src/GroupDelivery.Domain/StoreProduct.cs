using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GroupDelivery.Domain
{
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
}
