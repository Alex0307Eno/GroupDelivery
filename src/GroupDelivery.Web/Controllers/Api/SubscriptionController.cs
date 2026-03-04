using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;

        // 透過 DI 注入 Service
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("webhook")]
        public IActionResult PaymentWebhook([FromBody] PaymentWebhookDto payload)
        {
            _subscriptionService.HandlePaymentWebhook(payload);

            return Ok();
        }
    }
}