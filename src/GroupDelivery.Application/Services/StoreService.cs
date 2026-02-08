using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;


namespace GroupDelivery.Infrastructure.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IStoreClosedDateRepository _closedDateRepository;
        private readonly IStoreWeeklyClosedDayRepository _weeklyClosedDayRepo;
        private readonly IStoreRepository _storeRepository;


        public StoreService(IStoreRepository repo, IStoreClosedDateRepository closedDateRepository, IStoreWeeklyClosedDayRepository
         weeklyClosedDayRepo, IStoreRepository storeRepository)
        {
            _storeRepo = repo;
            _closedDateRepository = closedDateRepository;
            _weeklyClosedDayRepo = weeklyClosedDayRepo;
            _storeRepository = storeRepository;
        }
        #region 取得目前使用者擁有的所有商店
        public async Task<List<Store>> GetMyStoresAsync(int userId)
        {
            // 1. 從資料庫拿商店
            var stores = await _storeRepo.GetByOwnerAsync(userId);

            // 2. 現在時間（只取時分秒）
            var now = DateTime.Now.TimeOfDay;
            var today = DateTime.Today;

            // 3. 對每一家店計算是否營業
            foreach (var store in stores)
            {
                // === 休假判斷（最高優先權） ===
                if (store.IsOnHoliday)
                {
                    store.IsOpenNow = false;
                    continue;
                }

                if (store.HolidayStartDate.HasValue && store.HolidayEndDate.HasValue)
                {
                    if (today >= store.HolidayStartDate.Value.Date &&
                        today <= store.HolidayEndDate.Value.Date)
                    {
                        store.IsOpenNow = false;
                        continue;
                    }
                }

                // === 營業時間判斷 ===
                bool isWithinBusinessHours;

                // 一般營業（例如 09:00 ~ 18:00）
                if (store.OpenTime < store.CloseTime)
                {
                    isWithinBusinessHours =
                        now >= store.OpenTime &&
                        now <= store.CloseTime;
                }
                else
                {
                    // 跨日營業（例如 17:00 ~ 00:00）
                    isWithinBusinessHours =
                        now >= store.OpenTime ||
                        now <= store.CloseTime;
                }

                // === 最終結果 ===
                // 老闆手動接單 > 營業時間
                store.IsOpenNow =
                    store.IsAcceptingOrders ||
                    isWithinBusinessHours;
            }

            return stores;
        }

        #endregion

        #region 取得指定使用者名下的單一商店
        public async Task<Store> GetMyStoreAsync(int storeId, int userId)
        {
            return await _storeRepo.GetByIdAndOwnerAsync(storeId, userId);
        }
        #endregion

        #region 商店建立 / 更新 / 刪除
        public async Task<int> CreateAsync(int userId, StoreInitRequest request)
        {
            var store = new Store
            {
                OwnerUserId = userId,
                StoreName = request.StoreName,
                Phone = request.Phone,
                Address = request.Address,
                Description = request.Description,
                MinOrderAmount = request.MinOrderAmount,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime,
                IsAcceptingOrders = request.IsAcceptingOrders,
                Notice = request.Notice,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
            ApplyBusinessTimePreset(store, request);

            return await _storeRepo.CreateAsync(store);
        }


        public async Task UpdateAsync(int userId, StoreUpdateRequest request)
        {
            var store = await _storeRepo.GetByIdAndOwnerAsync(request.StoreId, userId);
            if (store == null)
                throw new Exception("Store not found");

            store.StoreName = request.StoreName;
            store.Phone = request.Phone;
            store.Address = request.Address;
            store.Description = request.Description;
            store.OpenTime = request.OpenTime;
            store.CloseTime = request.CloseTime;
            store.IsAcceptingOrders = request.IsAcceptingOrders;
            store.Notice = request.Notice;
            store.ModifiedAt = DateTime.UtcNow;
            store.IsOnHoliday = request.IsOnHoliday;
            store.HolidayStartDate = request.HolidayStartDate;
            store.HolidayEndDate = request.HolidayEndDate;

            ApplyBusinessTimePreset(store, request);

            await _storeRepo.UpdateAsync(store);
        }

        public async Task DeleteAsync(int userId, int storeId)
        {
            var store = await _storeRepo.GetByIdAndOwnerAsync(storeId, userId);
            if (store == null)
                throw new Exception("Store not found");

            await _storeRepo.DeleteAsync(store);
        }

        #endregion

        #region 商店圖片更新
        public async Task UpdateCoverImageAsync(int storeId, int ownerUserId,string url)
        {
            var store = await _storeRepo.GetByIdAndOwnerAsync(storeId, ownerUserId);
            store.CoverImageUrl = url;
            store.ModifiedAt = DateTime.UtcNow;
            await _storeRepo.UpdateAsync(store);
        }

        public async Task UpdateMenuImageAsync(int storeId, int ownerUserId, string url)
        {
            var store = await _storeRepo.GetByIdAndOwnerAsync(storeId, ownerUserId);
            store.MenuImageUrl = url;
            store.ModifiedAt = DateTime.UtcNow;
            await _storeRepo.UpdateAsync(store);
        }
        #endregion

        #region 計算商店在指定時間點的營業狀態
        public StoreOpenStatus GetStoreStatus(Store store, DateTime now)
        {
            if (!store.IsAcceptingOrders)
                return StoreOpenStatus.Paused;

            if (store.ClosedDates != null &&
                store.ClosedDates.Any(d => d.ClosedDate == now.Date))
                return StoreOpenStatus.Closed;

            var nowTime = now.TimeOfDay;
            if (nowTime < store.OpenTime || nowTime > store.CloseTime)
                return StoreOpenStatus.Closed;

            return StoreOpenStatus.Open;
        }
        #endregion

        #region 新增/刪除/更新指定日期的商店休息日
        public async Task AddClosedDateAsync(int storeId, int ownerUserId, DateTime closedDate)
        {
            var store = await _storeRepo.GetByIdAndOwnerAsync(storeId, ownerUserId);
            if (store == null)
                throw new Exception("Store not found");

            if (await _closedDateRepository.ExistsAsync(storeId, closedDate))
                return;

            await _closedDateRepository.AddAsync(new StoreClosedDate
            {
                StoreId = storeId,
                ClosedDate = closedDate.Date,
                CreatedAt = DateTime.UtcNow
            });
        }

        public async Task DeleteClosedDateAsync(int storeClosedDateId, int storeId, int ownerUserId)
        {
            var store = await _storeRepo.GetByIdAndOwnerAsync(storeId, ownerUserId);
            if (store == null)
                throw new Exception("Store not found");

            await _closedDateRepository.DeleteAsync(storeClosedDateId);
        }

        public async Task<Store> GetMyStoreWithClosedDatesAsync(int storeId, int ownerUserId)
        {
            // 1. 先確保這是我的店
            var store = await _storeRepo.GetByIdAndOwnerAsync(storeId, ownerUserId);
            if (store == null)
                return null;

            // 2. 再補上休息日
            store.ClosedDates =
                await _closedDateRepository.GetByStoreIdAsync(storeId);

            return store;
        }

        public async Task UpdateWeeklyClosedDaysAsync(int storeId,int ownerUserId,List<int> days)
        {
            var store = await _storeRepo.GetByIdAndOwnerAsync(storeId, ownerUserId);
            if (store == null)
                throw new Exception("Store not found");

            // 👇 真正幹活的是 Repository
            await _weeklyClosedDayRepo.ReplaceAsync(storeId, days);
        }
        #endregion

        #region 轉換營業時間
        // 1) 統一邏輯的 core，規則只寫這裡
        private void ApplyBusinessTimePresetCore(Store store, string preset, TimeSpan open, TimeSpan close)
        {
            switch (preset)
            {
                case "Morning":
                    store.OpenTime = new TimeSpan(8, 0, 0);
                    store.CloseTime = new TimeSpan(12, 0, 0);
                    break;

                case "Noon":
                    store.OpenTime = new TimeSpan(11, 0, 0);
                    store.CloseTime = new TimeSpan(14, 0, 0);
                    break;

                case "Afternoon":
                    store.OpenTime = new TimeSpan(14, 0, 0);
                    store.CloseTime = new TimeSpan(18, 0, 0);
                    break;

                case "Evening":
                    store.OpenTime = new TimeSpan(17, 0, 0);
                    store.CloseTime = new TimeSpan(22, 0, 0);
                    break;

                case "AllDay":
                    store.OpenTime = TimeSpan.Zero;
                    store.CloseTime = new TimeSpan(23, 59, 0);
                    break;

                case "Custom":
                default:
                    // 自訂就直接吃使用者填的
                    store.OpenTime = open;
                    store.CloseTime = close;
                    break;
            }
        }

        // 2) 給「新增」用的版本
        private void ApplyBusinessTimePreset(Store store, StoreInitRequest request)
        {
            ApplyBusinessTimePresetCore(
                store,
                request.BusinessTimePreset,
                request.OpenTime,
                request.CloseTime);
        }

        // 3) 給「修改」用的版本
        private void ApplyBusinessTimePreset(Store store, StoreUpdateRequest request)
        {
            ApplyBusinessTimePresetCore(
                store,
                request.BusinessTimePreset,
                request.OpenTime.Value,
                request.CloseTime.Value);
        }
        #endregion
    }
}
