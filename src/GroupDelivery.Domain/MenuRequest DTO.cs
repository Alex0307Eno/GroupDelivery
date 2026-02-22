using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class CreateOrderRequest
    {
        public int GroupOrderId { get; set; }

        public List<OrderItemRequest> Items { get; set; }
    }

    public class OrderItemRequest
    {
        public int StoreMenuItemId { get; set; }

        public int Quantity { get; set; }
    }
}
