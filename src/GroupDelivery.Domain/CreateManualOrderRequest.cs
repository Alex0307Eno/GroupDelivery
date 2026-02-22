using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class CreateManualOrderRequest
    {
        public int GroupOrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
