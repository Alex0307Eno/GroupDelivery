using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepo;

        public StoreService(IStoreRepository repo)
        {
            _storeRepo = repo;
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
    }
}
