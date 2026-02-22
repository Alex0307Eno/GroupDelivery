using GroupDelivery.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpGet("/orders/my")]
        public async Task<IActionResult> MyOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var orders = await _orderService.GetMyOrdersAsync(userId);

            return View(orders);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MerchantOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var orders = await _orderService.GetOrdersForMerchantAsync(userId);

            var grouped = orders
                .GroupBy(x => x.GroupOrderId)
                .ToList();

            return View(grouped);
        }
    }
}
