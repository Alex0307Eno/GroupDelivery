using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}