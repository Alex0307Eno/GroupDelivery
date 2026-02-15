using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/store-menu")]
    [Authorize(Roles = "Merchant")]
    public class StoreMenuAdvancedApiController : ControllerBase
    {
        private readonly IStoreMenuService _storeMenuService;

        public StoreMenuAdvancedApiController(IStoreMenuService storeMenuService)
        {
            _storeMenuService = storeMenuService;
        }

        [HttpPut("category/reorder")]
        public async Task<IActionResult> ReorderCategories([FromBody] List<CategoryReorderRequest> request)
        {
            await _storeMenuService.ReorderCategoriesAsync(GetUserId(), request);
            return Ok(ApiResponse.Ok());
        }

        [HttpPut("category/active")]
        public async Task<IActionResult> BatchUpdateCategoryActive([FromBody] List<CategoryActiveRequest> request)
        {
            await _storeMenuService.BatchSetCategoryActiveAsync(GetUserId(), request);
            return Ok(ApiResponse.Ok());
        }

        [HttpGet("items/available")]
        public async Task<IActionResult> GetAvailableItems([FromQuery] string time)
        {
            var data = await _storeMenuService.GetAvailableItemsAsync(GetUserId(), time);
            return Ok(ApiResponse.Ok(data));
        }

        [HttpPost("category/transfer")]
        public async Task<IActionResult> TransferCategory([FromBody] CategoryTransferRequest request)
        {
            await _storeMenuService.TransferCategoryAsync(GetUserId(), request);
            return Ok(ApiResponse.Ok());
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim.Value);
        }
    }
}
