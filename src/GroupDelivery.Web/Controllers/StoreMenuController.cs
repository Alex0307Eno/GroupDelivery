using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class StoreMenuController : Controller
    {
        private readonly IStoreMenuService _service;

        public StoreMenuController(IStoreMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Create(int storeId)
        {
            ViewBag.StoreId = storeId;
            return View();
        }

        [HttpGet("Manage/{storeId}")]
        public async Task<IActionResult> Manage(int storeId, bool? categoryIsActive)
        {
            // 管理頁顯示分類與菜單資料
            var viewModel = new StoreMenuManageViewModel
            {
                StoreId = storeId,
                CategoryIsActiveFilter = categoryIsActive,
                Categories = await _service.GetCategoriesAsync(storeId, categoryIsActive),
                Items = await _service.GetMenuAsync(storeId)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            await _service.ToggleActiveAsync(id);
            return RedirectToAction("Manage");
        }

        [HttpGet("BatchCreate/{storeId}")]
        public IActionResult BatchCreate(int storeId)
        {
            ViewBag.StoreId = storeId;
            return View();
        }
    }
}
