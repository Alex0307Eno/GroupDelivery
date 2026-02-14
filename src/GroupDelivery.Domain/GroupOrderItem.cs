using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class GroupOrderItem
    {
        public int GroupOrderItemId { get; set; }

        public int GroupOrderId { get; set; }

        public int StoreMenuItemId { get; set; }

        public int UserId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual GroupOrder GroupOrder { get; set; }

        public virtual StoreMenuItem StoreMenuItem { get; set; }

        public virtual User User { get; set; }
    }

}
