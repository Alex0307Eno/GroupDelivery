using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [Authorize]
        [HttpGet("stores/{storePublicId:guid}")]

        public async Task<IActionResult> GetMyStore(Guid storePublicId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            var store = await _storeService
                .GetMyStoreAsync(storePublicId, userId);

            if (store == null)
                return NotFound();

            return Ok(new
            {
                storePublicId = store.StorePublicId,
                storeName = store.StoreName,
                hasActiveGroupOrders = store.HasActiveGroupOrders
            });
        }
        [HttpGet("stores")]
        public async Task<IActionResult> GetMyStores()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            var stores = await _storeService.GetMyStoresAsync(userId);

            return Ok(stores.Select(x => new
            {
                storeId = x.StoreId,
                storeName = x.StoreName
            }));
        }
        [HttpGet("stores/{storeId}/delivery-threshold")]
        public async Task<IActionResult> GetDeliveryThreshold(int storeId)
        {
            var rules = await _deliveryRuleService.GetRulesByStoreAsync(storeId);

            if (rules == null || !rules.Any())
                return Ok(new List<object>());

            var result = rules
                .OrderBy(x => x.MaxDistanceKm)
                .Select(x => new
                {
                    maxDistanceKm = x.MaxDistanceKm,
                    minimumOrderAmount = x.MinimumOrderAmount,
                    deliveryFeeIfNotMet = x.DeliveryFeeIfNotMet
                });

            return Ok(result);
        }
    }
}
