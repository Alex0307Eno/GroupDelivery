using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        // 建立訂閱
        public string CreateSubscription(int merchantId, string planType)
        {
            decimal price = GetPlanPrice(planType);

            MerchantSubscription subscription = new MerchantSubscription();
            subscription.MerchantId = merchantId;
            subscription.PlanType = planType;
            subscription.Price = price;
            subscription.Status = "Pending";
            subscription.CreatedAt = DateTime.Now;
            subscription.UpdatedAt = DateTime.Now;

            _subscriptionRepository.Add(subscription);

            // 這裡回傳金流頁面URL
            string paymentUrl = "金流建立訂閱API";

            return paymentUrl;
        }

        // webhook
        public void HandlePaymentWebhook(PaymentWebhookDto payload)
        {
            // 根據金流回傳資料更新訂閱

            MerchantSubscription subscription =
                _subscriptionRepository.GetByGatewayId(payload.SubscriptionId);

            if (subscription == null)
            {
                return;
            }

            if (payload.Status == "SUCCESS")
            {
                subscription.Status = "Active";
                subscription.NextBillingDate = DateTime.Now.AddMonths(1);
            }
            else
            {
                subscription.Status = "Failed";
            }

            _subscriptionRepository.Update(subscription);
        }
        private decimal GetPlanPrice(string planType)
        {
            if (planType == "299")
                return 299;

            if (planType == "499")
                return 499;

            if (planType == "699")
                return 699;

            if (planType == "999")
                return 999;

            throw new Exception("Invalid Plan");
        }
    }
}
