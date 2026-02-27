using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static GroupDelivery.Web.Controllers.StoreMenuController;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/storemenu")]
    [Authorize(Roles = "Merchant")]
    public class StoreMenuApiController : ControllerBase
    {
        private readonly IStoreMenuService _storeMenuService;
        private readonly IStoreMenuCategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public StoreMenuApiController(IStoreMenuService storeMenuService, IStoreMenuCategoryService categoryService,IWebHostEnvironment environment)
        {
            _storeMenuService = storeMenuService;
            _categoryService = categoryService;
            _environment = environment;
        }



        [HttpPost("batchcreate")]
        public async Task<IActionResult> BatchCreate()
        {
            var form = Request.Form;

            if (!form.ContainsKey("storeId"))
                return BadRequest("StoreId 缺失");

            int storeId;
            if (!int.TryParse(form["storeId"], out storeId))
                return BadRequest("StoreId 格式錯誤");

            if (storeId <= 0)
                return BadRequest("StoreId 錯誤");

            var itemsJson = form["items"];

            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var items = System.Text.Json.JsonSerializer
                .Deserialize<List<MenuItemDto>>(itemsJson, options);

            if (items == null || !items.Any())
                return BadRequest("沒有菜單項目");

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            // 建立圖片對應表
            var imageMap = new Dictionary<int, string>();

            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "menu");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            for (int i = 0; i < form.Files.Count; i++)
            {
                var file = form.Files[i];

                if (file.Length == 0)
                    continue;

                // 前端是 itemImage_0, itemImage_1
                var key = file.Name;

                if (!key.StartsWith("itemImage_"))
                    continue;

                var indexStr = key.Replace("itemImage_", "");

                int index;
                if (!int.TryParse(indexStr, out index))
                    continue;

                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var fullPath = Path.Combine(uploadPath, newFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                imageMap[index] = "/uploads/menu/" + newFileName;
            }

            // 把圖片塞回 DTO
            for (int i = 0; i < items.Count; i++)
            {
                if (imageMap.ContainsKey(i))
                {
                    items[i].ImageUrl = imageMap[i];
                }
            }

            await _storeMenuService.BatchCreateAsync(userId, storeId, items);

            return Ok();
        }
        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] ToggleMenuRequest request)
        {
            if (request == null || request.Id <= 0)
                return BadRequest("Id 錯誤");

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            await _storeMenuService.ToggleActiveAsync(userId, request.Id);

            return Ok();
        }

        [HttpPost("category")]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("分類名稱不可空白");

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            await _categoryService.CreateAsync(userId, dto.StoreId, dto.Name);

            return Ok();
        }
        [HttpGet("categories/{storeId}")]
        public async Task<IActionResult> GetCategories(int storeId)
        {
            var list = await _categoryService.GetByStoreIdAsync(storeId);

            var result = list.Select(c => new
            {
                storeMenuCategoryId = c.StoreMenuCategoryId,
                name = c.Name
            });

            return Ok(result);
        }
        [HttpGet("list/{storeId}")]
        public async Task<IActionResult> List(int storeId)
        {
            var items = await _storeMenuService.GetMenuAsync(storeId);

            var result = items.Select(x => new
            {
                storeMenuItemId = x.StoreMenuItemId,
                imageUrl = x.ImageUrl,
                categoryId = x.CategoryId,
                name = x.Name,
                price = x.Price,
                description = x.Description
            });

            return Ok(result);
        }
        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] MenuItemEditDto dto)
        {
            if (dto == null)
                return BadRequest("資料不可為空");

            if (dto.StoreMenuItemId <= 0)
                return BadRequest("Id 錯誤");

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            await _storeMenuService.UpdateAsync(userId, dto);

            return Ok();
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteMenuRequest request)
        {
            if (request == null || request.Id <= 0)
                return BadRequest("Id 錯誤");

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            await _storeMenuService.DeleteAsync(userId, request.Id);

            return Ok();
        }

        public class DeleteMenuRequest
        {
            public int Id { get; set; }
        }
    }
}
