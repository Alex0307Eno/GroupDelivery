using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/storemenu")]
    [Authorize(Roles = "Merchant")]
    public class StoreMenuApiController : ControllerBase
    {
        private readonly IStoreMenuService _service;

        public StoreMenuApiController(IStoreMenuService service)
        {
            _service = service;
        }

        [HttpPost("batch")]
        public async Task<IActionResult> BatchCreate([FromBody] BatchMenuRequest request)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _service.BatchCreateAsync(
                userId,
                request.StoreId,
                request.Items);

            return Ok();
        }
    }
}
