using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/orders")]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderApiController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return BadRequest("訂單資料錯誤");

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            await _orderService.CreateOrderAsync(userId, request);

            return Ok();
        }
        [Authorize]
        [HttpPost("manual")]
        public async Task<IActionResult> CreateManual([FromBody] CreateManualOrderRequest request)
        {
            if (request == null || request.Amount <= 0)
                return BadRequest("金額錯誤");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _orderService.CreateManualOrderAsync(userId, request);

            return Ok();
        }
        [Authorize]
        [HttpGet("all-orders")]
        public async Task<IActionResult> GetMerchantOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var orders = await _orderService.GetOrdersForMerchantAsync(userId);

            // 團單：有 GroupOrderId 的
            var groupOrders = orders
                .Where(o => o.GroupOrderId != null)
                .GroupBy(o => o.GroupOrderId)
                .ToList();

            // 客製化單：沒有 GroupOrderId 的
            var customOrders = orders
                .Where(o => o.GroupOrderId == null)
                .ToList();

            var today = DateTime.Today;

            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var startOfWeek = today.AddDays(-diff);
            var endOfWeek = startOfWeek.AddDays(7);

            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var result = new
            {
                // 團購訂單依截止時間分區段
                today = groupOrders
                    .Where(g => g.First().GroupOrder.Deadline.Date == today),

                week = groupOrders
                    .Where(g =>
                        g.First().GroupOrder.Deadline >= startOfWeek &&
                        g.First().GroupOrder.Deadline < endOfWeek),

                month = groupOrders
                    .Where(g =>
                        g.First().GroupOrder.Deadline >= startOfMonth &&
                        g.First().GroupOrder.Deadline < endOfMonth),

                // 客製化訂單全部丟過去，前端另外一區顯示
                custom = customOrders
            };

            return Ok(result);
        }
    }
}