using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreService
    {
        Task<List<Store>> GetMyStoresAsync(int userId);
        Task<Store> GetMyStoreAsync(int storeId, int userId);
        Task<int> CreateAsync(int userId, StoreInitRequest request);
        Task UpdateAsync(int userId, StoreUpdateRequest request);
        Task DeleteAsync(int userId, int storeId);
        Task UpdateCoverImageAsync(int storeId, int ownerUserId, string coverImageUrl);
        Task UpdateMenuImageAsync(int storeId, int ownerUserId, string menuImageUrl);
        Task<Store> GetFirstByOwnerAsync(int ownerUserId);
        Task<Store> GetByIdAsync(int storeId);
        Task<List<NearbyStoreDto>> GetNearbyStoresAsync();


    }
}
