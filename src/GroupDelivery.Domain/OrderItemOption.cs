using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class OrderItemOption
    {
        public int OrderItemOptionId { get; set; }

        public int OrderItemId { get; set; }

        public string OptionName { get; set; }

        public decimal PriceAdjust { get; set; }

        public virtual OrderItem OrderItem { get; set; }
    }
}
