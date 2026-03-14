using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreRepository
    {
        // 取得指定使用者的指定商店
        Task<Store> GetByIdAndOwnerAsync(int storeId, int ownerUserId);

        // 取得使用者的第一間商店
        Task<Store> GetFirstByOwnerAsync(int ownerUserId);
        Task<Store> GetByPublicIdAndOwnerAsync(Guid publicId, int ownerUserId); 

        // 取得使用者的所有商店
        Task<List<Store>> GetByOwnerAsync(int ownerUserId);

        // CRUD
        Task<Guid> CreateAsync(Store store);
        Task UpdateAsync(Store store);
        Task DeleteAsync(Store store);

        Task<Store> GetByIdAsync(int storeId);
        Task<Store> GetByGuIdAsync(Guid StoreMenuItemPublicId);
        Task<List<Store>> GetAllAsync();
        Task<Store> GetByPublicIdAsync(Guid storePublicId);
        Task<List<Store>> GetByOwnerUserIdAsync(int userId);
        Task<List<StoreNearbyDto>> GetNearbyStoresAsync(double? lat, double? lng, string city);




    }
}
