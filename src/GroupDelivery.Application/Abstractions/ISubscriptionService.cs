using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface ISubscriptionService
    {
        string CreateSubscription(int merchantId, string planType);
        void HandlePaymentWebhook(PaymentWebhookDto payload);
    }
}
