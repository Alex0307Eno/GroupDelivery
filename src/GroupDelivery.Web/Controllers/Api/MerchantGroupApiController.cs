using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
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
    [Route("api/merchant")]
    [Authorize]
    public class MerchantGroupApiController : ControllerBase
    {
        private readonly IGroupOrderService _groupOrderService;
        private readonly IStoreService _storeService;
        private readonly IDeliveryRuleService _deliveryRuleService;

        public MerchantGroupApiController(IGroupOrderService groupOrderService, IStoreService storeService, IDeliveryRuleService deliveryRuleService)
        {
            _groupOrderService = groupOrderService;
            _storeService = storeService;
            _deliveryRuleService = deliveryRuleService;
        }

        [HttpPost("groups")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
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
        public async Task<IActionResult> GetMyStores()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var stores = await _storeService.GetMyStoresAsync(userId);

            var result = stores.Select(s => new
            {
                storeId = s.StoreId,
                storeName = s.StoreName,
                hasActiveGroupOrders = s.HasActiveGroupOrders
            });

            return Ok(result);
        }
        [HttpGet("stores/{storeId}/delivery-threshold")]
        public async Task<IActionResult> GetDeliveryThreshold(int storeId)
        {
            var rules = await _deliveryRuleService.GetByStoreIdAsync(storeId);

            if (rules == null || !rules.Any())
                return Ok(new { targetAmount = 0 });

            // 取最大距離那筆門檻（最保守）
            var maxRule = rules
                .OrderByDescending(x => x.MaxDistanceKm)
                .First();

            return Ok(new
            {
                targetAmount = maxRule.MinimumOrderAmount
            });
        }
    }
}
