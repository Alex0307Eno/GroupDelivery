using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Exceptions;
using GroupDelivery.Domain;
using GroupDelivery.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/store-menu")]
    [Authorize(Roles = "Merchant")]
    public class StoreMenuApiController : ControllerBase
    {
        private readonly IStoreMenuService _service;
        private readonly ICurrentStoreContextService _currentStoreContextService;
        private readonly IStoreService _storeService;

        public StoreMenuApiController(
            IStoreMenuService service,
            ICurrentStoreContextService currentStoreContextService,
            IStoreService storeService)
        {
            _service = service;
            _currentStoreContextService = currentStoreContextService;
            _storeService = storeService;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var storeId = _currentStoreContextService.GetCurrentStoreId(User);
                var items = await _service.GetMenuAsync(storeId);

                var result = items.Select(ToApiModel).ToList();
                return Ok(new { success = true, data = result });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("items/available")]
        public async Task<IActionResult> GetAvailableItems([FromQuery] string time)
        {
            try
            {
                var storeId = _currentStoreContextService.GetCurrentStoreId(User);
                TimeSpan? targetTime = null;

                if (!string.IsNullOrWhiteSpace(time))
                {
                    TimeSpan parsed;
                    if (!TimeSpan.TryParse(time, out parsed))
                        throw new BusinessException("time 格式必須為 HH:mm");

                    targetTime = parsed;
                }

                var items = await _service.GetAvailableMenuItemsAsync(storeId, targetTime);
                var result = items.Select(ToApiModel).ToList();
                return Ok(new { success = true, data = result });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("category")]
        public async Task<IActionResult> GetCategories([FromQuery] bool? isActive)
        {
            try
            {
                var storeId = _currentStoreContextService.GetCurrentStoreId(User);
                var categories = await _service.GetCategoriesAsync(storeId, isActive);

                return Ok(new
                {
                    success = true,
                    data = categories.Select(x => new
                    {
                        categoryId = x.Id,
                        name = x.Name,
                        sortOrder = x.SortOrder,
                        isActive = x.IsActive
                    })
                });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("category/reorder")]
        public async Task<IActionResult> ReorderCategories([FromBody] List<CategoryReorderRequest> requests)
        {
            try
            {
                var storeId = _currentStoreContextService.GetCurrentStoreId(User);
                await _service.ReorderCategoriesAsync(storeId, requests);
                return Ok(new { success = true });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("category/active")]
        public async Task<IActionResult> ToggleCategoryActive([FromBody] List<CategoryActiveUpdateRequest> requests)
        {
            try
            {
                var storeId = _currentStoreContextService.GetCurrentStoreId(User);
                await _service.ToggleCategoryActiveAsync(storeId, requests);
                return Ok(new { success = true });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("category/transfer")]
        public async Task<IActionResult> TransferCategory([FromBody] CategoryTransferRequest request)
        {
            try
            {
                var storeId = _currentStoreContextService.GetCurrentStoreId(User);
                await _service.TransferCategoryItemsAsync(storeId, request);
                return Ok(new { success = true });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("batchcreate")]
        public async Task<IActionResult> BatchCreate()
        {
            try
            {
                var form = Request.Form;
                var storeId = _currentStoreContextService.GetCurrentStoreId(User);

                var itemsJson = form["items"];
                var items = System.Text.Json.JsonSerializer.Deserialize<List<MenuItemDto>>(itemsJson);
                if (items == null || !items.Any())
                    throw new BusinessException("沒有菜單項目");

                await _service.BatchCreateAsync(storeId, items);

                var file = form.Files.FirstOrDefault();
                if (file != null)
                {
                    _storeService.ValidateImage(file.FileName, file.Length);

                    var path = Path.Combine("wwwroot/uploads", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                return Ok(new { success = true });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        private StoreMenuItemApiModel ToApiModel(StoreMenuItem item)
        {
            return new StoreMenuItemApiModel
            {
                StoreMenuItemId = item.StoreMenuItemId,
                StoreId = item.StoreId,
                CategoryId = item.CategoryId,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                IsActive = item.IsActive,
                ImageUrl = item.ImageUrl,
                CreatedAt = item.CreatedAt,
                AvailableStartTime = item.AvailableStartTime,
                AvailableEndTime = item.AvailableEndTime,
                RowVersion = item.RowVersion == null ? string.Empty : Convert.ToBase64String(item.RowVersion)
            };
        }

        public class StoreMenuItemApiModel
        {
            public int StoreMenuItemId { get; set; }
            public int StoreId { get; set; }
            public int? CategoryId { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }
            public string ImageUrl { get; set; }
            public DateTime CreatedAt { get; set; }
            public TimeSpan? AvailableStartTime { get; set; }
            public TimeSpan? AvailableEndTime { get; set; }
            public string RowVersion { get; set; }
        }
    }
}
