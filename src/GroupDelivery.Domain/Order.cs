using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int GroupOrderId { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}