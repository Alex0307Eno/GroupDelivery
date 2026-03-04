using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDelivery.Domain;


namespace GroupDelivery.Application.Abstractions
{
   
    public interface ISubscriptionRepository
    {
        void Add(MerchantSubscription subscription);

        MerchantSubscription GetById(int id);

        MerchantSubscription GetByGatewayId(string gatewaySubscriptionId);

        void Update(MerchantSubscription subscription);
    }
}
