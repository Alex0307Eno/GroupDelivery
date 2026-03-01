using GroupDelivery.Application.Abstractions;
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
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly IWebHostEnvironment _env;
        private readonly IGroupService _groupService;
        private readonly IGroupOrderService _groupOrderService;


        public StoreController(
       IStoreService storeService,
       IWebHostEnvironment env,
       IGroupService groupService,
       IGroupOrderService groupOrderService)
        {
            _storeService = storeService;
            _env = env;
            _groupService = groupService;
            _groupOrderService = groupOrderService;
        }
        // StoreController.cs
        public IActionResult CreateGroup()
        {
            return View();
        }
        [Authorize(Roles = "Merchant")]
        [HttpPost]
        public async Task<IActionResult> CreateGroup(int? storeId)
        {
            var userId = GetUserId();

           

            // === 商家自己開團 ===
            if (User.IsInRole("Merchant"))
            {
                var myStores = await _storeService.GetMyStoresAsync(userId);

                var vm = new CreateGroupRequest
                {
                    AvailableStores = myStores,
                    IsLockedStore = false
                };

                return View(vm);
            }

            return BadRequest();
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

            // 1️⃣ 建立商店（純商業邏輯）
            var storeId = await _storeService.CreateAsync(userId, request);
            // 2️⃣ 處理封面圖片（Web 邊界責任）
            if (request.CoverImage != null && request.CoverImage.Length > 0)
            {
                var coverUrl = await SaveStoreImage(
                    storeId,
                    request.CoverImage,
                    "cover");

                await _storeService.UpdateCoverImageAsync(storeId, userId, coverUrl);
            }

           

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

            var model = new StoreUpdateRequest
            {
                StoreId = store.StoreId,
                StoreName = store.StoreName,
                Address = store.Address,
                Landline = store.Landline,
                Mobile = store.Mobile,
                Description = store.Description,
                CurrentCoverImageUrl = store.CoverImageUrl,
                OpenTime = store.OpenTime,
                CloseTime = store.CloseTime,
                OpenTime2 = store.OpenTime2,
                CloseTime2 = store.CloseTime2,
                ClosedDays = string.IsNullOrEmpty(store.ClosedDays)
           ? new List<int>()
           : store.ClosedDays.Split(',').Select(int.Parse).ToList(),
                IsPausedToday=store.IsPausedToday
            };


            return View(model);
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

            // 1️⃣ 更新基本資料
            await _storeService.UpdateAsync(userId, request);

            // 2️⃣ 如果有上傳新封面
            if (request.NewCoverImage != null && request.NewCoverImage.Length > 0)
            {
                var coverUrl = await SaveStoreImage(
                    request.StoreId,
                    request.NewCoverImage,
                    "cover");

                await _storeService.UpdateCoverImageAsync(
                    request.StoreId,
                    userId,
                    coverUrl);
            }

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
        private async Task<string> SaveStoreImage(
    int storeId,
    IFormFile file,
    string prefix)
        {
            var folder = Path.Combine(
                _env.WebRootPath,
                "uploads",
                "stores",
                storeId.ToString());

            Directory.CreateDirectory(folder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{prefix}_{Guid.NewGuid()}{ext}";
            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/stores/{storeId}/{fileName}";
        }


        [Authorize]
        public async Task<IActionResult> MyGroups()
        {
            var userId = GetUserId();

            List<GroupOrder> groups =
                await _groupOrderService.GetMyGroupOrdersAsync(userId);

            return View(groups);
        }

        [HttpGet]
        public async Task<IActionResult> StoreGroups(int storeId)
        {
            var groups = await _groupOrderService
                .GetOpenGroupsByStoreAsync(storeId);

            ViewData["StoreId"] = storeId;

            return View(groups);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> TogglePause(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _storeService.TogglePauseAsync(userId, id);

            return RedirectToAction(nameof(MyStores));
        }
    }
}
