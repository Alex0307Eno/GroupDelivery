using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

            var today = DateTime.Today;

            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startOfWeek = today.AddDays(-diff);
            var endOfWeek = startOfWeek.AddDays(7);

            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var vm = new MerchantOrderViewModel
            {
                TodayGroups = grouped
                    .Where(g => g.First().GroupOrder.Deadline.Date == today)
                    .ToList(),

                WeekGroups = grouped
                    .Where(g =>
                        g.First().GroupOrder.Deadline >= startOfWeek &&
                        g.First().GroupOrder.Deadline < endOfWeek)
                    .ToList(),

                MonthGroups = grouped
                    .Where(g =>
                        g.First().GroupOrder.Deadline >= startOfMonth &&
                        g.First().GroupOrder.Deadline < endOfMonth)
                    .ToList()
            };

            return View(vm);
        }
    }
}
