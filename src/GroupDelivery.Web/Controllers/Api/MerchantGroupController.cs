using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/merchant")]
    [Authorize]
    public class MerchantGroupController : ControllerBase
    {
        private readonly IGroupOrderService _groupOrderService;

        public MerchantGroupController(IGroupOrderService groupOrderService)
        {
            _groupOrderService = groupOrderService;
        }

        [HttpPost("groups")]
        public async Task<IActionResult> CreateGroup(
            [FromBody] CreateGroupRequest request)
        {
            if (request == null)
                return BadRequest("請求資料為空");

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = int.Parse(claim.Value);

            try
            {
                await _groupOrderService.CreateGroupAsync(userId, request);
                return Ok(new { message = "開團成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
