using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class PaymentWebhookDto
    {
        public string SubscriptionId { get; set; }
        public string TradeNo { get; set; }

        public string Status { get; set; }

        public decimal Amount { get; set; }
    }
}