using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenuItemController(
            IMenuItemService menuItemService,
            IWebHostEnvironment webHostEnvironment)
        {
            _menuItemService = menuItemService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId)
        {
            var ownerUserId = GetUserId();
            var items = await _menuItemService.GetActiveAsync(ownerUserId, categoryId);
            var deletedItems = await _menuItemService.GetDeletedAsync(ownerUserId);
            var categories = await _menuItemService.GetCategoriesAsync(ownerUserId);

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name", categoryId);
            ViewBag.DeletedItems = deletedItems;
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(string name)
        {
            try
            {
                var ownerUserId = GetUserId();
                await _menuItemService.CreateCategoryAsync(ownerUserId, name);
            }
            catch (Exception ex)
            {
                TempData["MenuError"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var ownerUserId = GetUserId();
            await SetCategoryViewBagAsync(ownerUserId, null);
            return View(new MenuItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem item, IFormFile imageFile)
        {
            var ownerUserId = GetUserId();

            if (imageFile != null && imageFile.Length > 0)
            {
                item.ImageUrl = await SaveImageAsync(imageFile);
            }

            if (!ModelState.IsValid)
            {
                await SetCategoryViewBagAsync(ownerUserId, item.CategoryId);
                return View(item);
            }

            try
            {
                await _menuItemService.CreateAsync(ownerUserId, item);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await SetCategoryViewBagAsync(ownerUserId, item.CategoryId);
                return View(item);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ownerUserId = GetUserId();
            var item = await _menuItemService.GetByIdAsync(ownerUserId, id);

            if (item == null || item.IsDeleted)
            {
                return NotFound();
            }

            await SetCategoryViewBagAsync(ownerUserId, item.CategoryId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuItem item, IFormFile imageFile)
        {
            var ownerUserId = GetUserId();

            if (imageFile != null && imageFile.Length > 0)
            {
                item.ImageUrl = await SaveImageAsync(imageFile);
            }
            else
            {
                var current = await _menuItemService.GetByIdAsync(ownerUserId, item.Id);
                item.ImageUrl = current == null ? null : current.ImageUrl;
            }

            if (!ModelState.IsValid)
            {
                await SetCategoryViewBagAsync(ownerUserId, item.CategoryId);
                return View(item);
            }

            try
            {
                await _menuItemService.UpdateAsync(ownerUserId, item);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await SetCategoryViewBagAsync(ownerUserId, item.CategoryId);
                return View(item);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ownerUserId = GetUserId();
            await _menuItemService.DeleteAsync(ownerUserId, id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var ownerUserId = GetUserId();
            await _menuItemService.RestoreAsync(ownerUserId, id);
            return RedirectToAction("Index");
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        private async Task SetCategoryViewBagAsync(int ownerUserId, int? selectedCategoryId)
        {
            var categories = await _menuItemService.GetCategoriesAsync(ownerUserId);
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name", selectedCategoryId);
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            // 採用本機檔案儲存，路徑固定於 wwwroot/uploads/menu。
            var uploadRoot = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "menu");
            Directory.CreateDirectory(uploadRoot);

            var extension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString("N") + extension;
            var fullPath = Path.Combine(uploadRoot, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/menu/" + fileName;
        }
    }
}
