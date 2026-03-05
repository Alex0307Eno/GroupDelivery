using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class DeliveryRuleDto
    {
        public decimal MaxDistanceKm { get; set; }

        public decimal MinimumOrderAmount { get; set; }

        public decimal DeliveryFeeIfNotMet { get; set; }
    }
}
