using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Services;
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
        private readonly IStoreService _storeService;
        private readonly IWebHostEnvironment _environment;
        private readonly IStoreMenuCategoryService _storeMenuCategoryService;

        public StoreMenuController(IStoreMenuService storeMenuService, IStoreService storeService, IWebHostEnvironment environment, IStoreMenuCategoryService storeMenuCategoryService)
        {
            _storeMenuService = storeMenuService;
            _storeService = storeService;
            _environment = environment;
            _storeMenuCategoryService = storeMenuCategoryService;
        }




        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
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

                OptionGroups = item.OptionGroups?
                    .Select(g => new StoreMenuItemOptionGroupDto
                    {
                        StoreMenuItemOptionGroupId = g.StoreMenuItemOptionGroupId,
                        GroupName = g.GroupName,
                        Options = g.Options.Select(o => new StoreMenuItemOptionDto
                        {
                            StoreMenuItemOptionId = o.StoreMenuItemOptionId,
                            OptionName = o.OptionName,
                            PriceAdjust = o.PriceAdjust
                        }).ToList()
                    }).ToList() ?? new List<StoreMenuItemOptionGroupDto>()
            };

            var categories = await _storeMenuCategoryService
                .GetByStoreIdAsync(item.StoreId);

            ViewBag.Categories = categories;

            return View(dto);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(MenuItemEditDto dto, IFormFile imageFile)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "menu");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                var fullPath = Path.Combine(uploadPath, newFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                dto.ImageUrl = "/uploads/menu/" + newFileName;
            }

            await _storeMenuService.UpdateAsync(userId, dto);

            return RedirectToAction("Manage", new { storePublicId = dto.StoreId });
        }


        [HttpGet("Manage/{storePublicId:guid}")]
        public async Task<IActionResult> Manage(Guid storePublicId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            var store = await _storeService.GetMyStoreAsync(storePublicId, userId);

            if (store == null)
                return NotFound();

            ViewBag.StoreId = store.StoreId;
            ViewBag.StorePublicId = store.StorePublicId;

            var items = await _storeMenuService.GetMenuAsync(store.StoreId);

            return View(items);
        }

        [Authorize(Roles = "Merchant")]
        [HttpGet("BatchCreate/{storePublicId:guid}")]
        public IActionResult BatchCreate(Guid storePublicId)
        {
            if (storePublicId == Guid.Empty)
            {
                return NotFound();
            }

            return View(model: storePublicId);
        }

        


        public class ToggleMenuRequest
        {
            public Guid PublicId { get; set; }
        }

    }
}
