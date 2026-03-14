using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreService
    {
        Task<List<Store>> GetMyStoresAsync(int userId);
        Task<Store> GetMyStoreAsync(Guid storePublicId, int userId);
        Task<Guid> CreateAsync(int userId, StoreInitRequest request);
        Task UpdateAsync(int userId, StoreUpdateRequest request);
        Task DeleteAsync(int userId, int storeId);
        Task UpdateCoverImageAsync(Guid storeId, int ownerUserId, string coverImageUrl);
        Task UpdateMenuImageAsync(int storeId, int ownerUserId, string menuImageUrl);
        Task<Store> GetFirstByOwnerAsync(int ownerUserId);
        Task<Store> GetByIdAsync(int storeId);
        Task<List<NearbyStoreDto>> GetNearbyStoresAsync();
        Task<List<StoreNearbyDto>> GetNearbyStoresAsync(double? lat, double? lng, string city);

        Task TogglePauseAsync(int userId, int storeId);
        Task<Store> GetByPublicIdAsync(Guid storePublicId);
        Task<List<StoreListItemViewModel>> GetMyStoreListAsync(int userId);

    }
}
