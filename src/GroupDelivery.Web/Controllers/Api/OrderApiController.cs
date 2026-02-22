using GroupDelivery.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GroupDelivery.Application.Abstractions;
using System.Linq;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
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
    }
}