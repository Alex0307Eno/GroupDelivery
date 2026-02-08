using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GroupDelivery.Infrastructure.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IStoreClosedDateRepository _closedDateRepository;


        public StoreService(IStoreRepository repo, IStoreClosedDateRepository closedDateRepository)
        {
            _storeRepo = repo;
            _closedDateRepository = closedDateRepository;
        }

        public async Task<List<Store>> GetMyStoresAsync(int userId)
        {
            return await _storeRepo.GetByOwnerAsync(userId);
        }

        public async Task<Store> GetMyStoreAsync(int storeId, int userId)
        {
            return await _storeRepo.GetByIdAndOwnerAsync(storeId, userId);
        }

        public async Task<int> CreateAsync(int userId, StoreInitRequest request)
        {
            var store = new Store
            {
                OwnerUserId = userId,
                StoreName = request.StoreName,
                Phone = request.Phone,
                Address = request.Address,
                Description = request.Description,
                OpenTime = request.OpenTime,
                CloseTime = request.CloseTime,
                IsAcceptingOrders = request.IsAcceptingOrders,
                Notice = request.Notice,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

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

            await _storeRepo.UpdateAsync(store);
        }

        public async Task DeleteAsync(int userId, int storeId)
        {
            var store = await _storeRepo.GetByIdAndOwnerAsync(storeId, userId);
            if (store == null)
                throw new Exception("Store not found");

            await _storeRepo.DeleteAsync(store);
        }
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

    }
}
