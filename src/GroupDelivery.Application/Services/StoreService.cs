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
        private readonly IGroupOrderRepository _groupOrderRepository;

        public StoreService(IStoreRepository storeRepo, IGroupOrderRepository groupOrderRepository)
        {
            _storeRepo = storeRepo;
            _groupOrderRepository = groupOrderRepository;
        }
        public async Task<Store> GetFirstByOwnerAsync(int ownerUserId)
        {
            return await _storeRepo.GetFirstByOwnerAsync(ownerUserId);
        }
        public async Task<Store> GetByIdAsync(int storeId)
        {
            return await _storeRepo.GetByIdAsync(storeId);
        }


        public async Task<List<Store>> GetMyStoresAsync(int userId)
        {
            var stores = await _storeRepo.GetByOwnerAsync(userId);
            var activeGroupOrders = await _groupOrderRepository.GetAllActiveAsync();
            var activeStoreIds = activeGroupOrders
                .Select(x => x.StoreId)
                .Distinct()
                .ToHashSet();

            foreach (var store in stores)
            {
                store.HasActiveGroupOrders = activeStoreIds.Contains(store.StoreId);
            }

            return stores;
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

        public async Task UpdateCoverImageAsync(int storeId, string url)
        {
            var store = await _storeRepo.GetByIdAsync(storeId);
            if (store == null)
                throw new Exception("Store not found");

            store.CoverImageUrl = url;
            store.ModifiedAt = DateTime.UtcNow;
            await _storeRepo.UpdateAsync(store);
        }

        public async Task UpdateMenuImageAsync(int storeId, string url)
        {
            var store = await _storeRepo.GetByIdAsync(storeId);
            if (store == null)
                throw new Exception("Store not found");

            store.MenuImageUrl = url;
            store.ModifiedAt = DateTime.UtcNow;
            await _storeRepo.UpdateAsync(store);
        }
        public async Task<List<NearbyStoreDto>> GetNearbyStoresAsync()
        {
            var now = DateTime.Now;

            var stores = await _storeRepo.GetAllAsync();
            var openGroups = await _groupOrderRepository.GetOpenGroupsAsync(now);

            var grouped = openGroups
                .GroupBy(g => g.StoreId)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Deadline = g.Min(x => x.Deadline),
                        CreatedAt = g
                            .OrderByDescending(x => x.CreatedAt)
                            .Select(x => x.CreatedAt)
                            .FirstOrDefault()
                    });

            var result = stores
                .Select(s => new NearbyStoreDto
                {
                    StoreId = s.StoreId,
                    StoreName = s.StoreName,
                    Address = s.Address,
                    Distance = 0,
                    HasActiveGroup = grouped.ContainsKey(s.StoreId),
                    ActiveGroupDeadline = grouped.ContainsKey(s.StoreId)
                        ? grouped[s.StoreId].Deadline
                        : (DateTime?)null,
                    ActiveGroupCreatedAt = grouped.ContainsKey(s.StoreId)
                        ? grouped[s.StoreId].CreatedAt
                        : (DateTime?)null,
                    CoverImageUrl = s.CoverImageUrl
                })
                .ToList();

            return result;
        }

        public void ValidateImage(string fileName, long fileSize)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("未選擇檔案");

            const long maxSize = 2 * 1024 * 1024;
            if (fileSize <= 0 || fileSize > maxSize)
                throw new Exception("檔案大小不可超過 2MB");

            var safeName = fileName.Trim();
            var parts = safeName.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new Exception("檔名格式錯誤，禁止雙副檔名");

            var ext = parts[1].ToLowerInvariant();
            if (ext != "jpg" && ext != "jpeg" && ext != "png")
                throw new Exception("僅支援 jpg/jpeg/png");
        }
    }
}
