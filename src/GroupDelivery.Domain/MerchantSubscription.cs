using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class MerchantSubscription
    {
        public int Id { get; set; }

        public int MerchantId { get; set; }

        public string PlanType { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        public string GatewaySubscriptionId { get; set; }

        public DateTime NextBillingDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
