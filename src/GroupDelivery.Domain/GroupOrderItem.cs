using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GroupDelivery.Domain 
{ 

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
