using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers
{
    [Route("StoreMenu")]
    [Authorize(Roles = "Merchant")]
    public class StoreMenuController : Controller
    {
        private readonly IStoreMenuService _storeMenuService;
        private readonly IWebHostEnvironment _environment;
        private readonly IStoreMenuCategoryService _storeMenuCategoryService;

        public StoreMenuController(IStoreMenuService storeMenuService, IWebHostEnvironment environment, IStoreMenuCategoryService storeMenuCategoryService)
        {
            _storeMenuService = storeMenuService;
            _environment = environment;
            _storeMenuCategoryService = storeMenuCategoryService;
        }

        


        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _storeMenuService.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            var dto = new MenuItemEditDto
            {
                StoreMenuItemId = item.StoreMenuItemId,
                StoreId = item.StoreId,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                CategoryId = item.CategoryId ?? 0,
                IsActive = item.IsActive,
                ImageUrl = item.ImageUrl,

                OptionGroups = item.OptionGroups.Select(g => new StoreMenuItemOptionGroupDto
                {
                    StoreMenuItemOptionGroupId = g.StoreMenuItemOptionGroupId,
                    GroupName = g.GroupName,
                    Options = g.Options.Select(o => new StoreMenuItemOptionDto
                    {
                        StoreMenuItemOptionId = o.StoreMenuItemOptionId,
                        OptionName = o.OptionName,
                        PriceAdjust = o.PriceAdjust
                    }).ToList()
                }).ToList()
        
            };

            var categories = await _storeMenuCategoryService.GetByStoreIdAsync(item.StoreId);

            ViewBag.Categories = categories;

            return View(dto);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(MenuItemEditDto dto, IFormFile ImageUrl)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "menu");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);

                var fullPath = Path.Combine(uploadPath, newFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ImageUrl.CopyToAsync(stream);
                }

                dto.ImageUrl = "/uploads/menu/" + newFileName;
            }

            await _storeMenuService.UpdateAsync(userId, dto);

            return RedirectToAction("Manage", new { storeId = dto.StoreId });
        }


        [HttpGet("Manage/{storeId}")]
        public async Task<IActionResult> Manage(int storeId)
        {
            ViewBag.StoreId = storeId; 

            var items = await _storeMenuService.GetMenuAsync(storeId);

            return View(items);
        }

        
        [HttpGet("BatchCreate/{storeId}")]
        public IActionResult BatchCreate(int storeId)
        {
            if (storeId <= 0)
            {
                return NotFound();
            }

            
            return View(model: storeId);
        }



        
        public class ToggleMenuRequest
        {
            public int Id { get; set; }
        }

    }
}
