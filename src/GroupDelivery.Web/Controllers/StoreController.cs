using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        // =========================
        // 列表：我的商店
        // =========================
        public async Task<IActionResult> MyStores()
        {
            var userId = GetUserId();
            var stores = await _storeService.GetMyStoresAsync(userId);
            return View(stores);
        }

        // =========================
        // 新增頁
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View(new StoreInitRequest());
        }

        // =========================
        // 新增送出
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreInitRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var userId = GetUserId();
            await _storeService.CreateAsync(userId, request);

            return RedirectToAction(nameof(MyStores));
        }

        // =========================
        // 編輯頁
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserId();
            var store = await _storeService.GetMyStoreAsync(id, userId);

            if (store == null)
                return NotFound();

            var vm = new StoreUpdateRequest
            {
                StoreId = store.StoreId,
                StoreName = store.StoreName,
                Phone = store.Phone,
                Address = store.Address,
                OpenTime = store.OpenTime,
                CloseTime = store.CloseTime,
                IsAcceptingOrders = store.IsAcceptingOrders,
                MinOrderAmount = store.MinOrderAmount,
                Notice = store.Notice
            };

            return View(vm);
        }

        // =========================
        // 編輯送出
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StoreUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var userId = GetUserId();
            await _storeService.UpdateAsync(userId, request);

            return RedirectToAction(nameof(MyStores));
        }

        // =========================
        // 刪除
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            await _storeService.DeleteAsync(userId, id);

            return RedirectToAction(nameof(MyStores));
        }

        // =========================
        // 共用：取得登入者 ID
        // =========================
        private int GetUserId()
        {
            return int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );
        }
    }
}
