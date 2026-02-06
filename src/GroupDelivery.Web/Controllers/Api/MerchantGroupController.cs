using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;


namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/merchant")]
    [Authorize]
    public class MerchantGroupController : ControllerBase
    {
        private readonly IGroupOrderService _groupOrderService;
        private readonly IStoreRepository _storeRepository;

        public MerchantGroupController(IGroupOrderService groupOrderService, IStoreRepository storeRepository)
        {
            _groupOrderService = groupOrderService;
            _storeRepository = storeRepository;
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
        [HttpGet("stores")]
        public IActionResult GetMyStores()
        {
            // 一定要用正確的 Claim
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var stores = _storeRepository.GetByOwnerUserId(userId);

            var result = stores.Select(s => new
            {
                storeId = s.StoreId,
                storeName = s.StoreName
            });

            return Ok(result);
        }
    }
}
