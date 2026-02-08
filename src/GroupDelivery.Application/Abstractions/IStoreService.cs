using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreService
    {
        Task<List<Store>> GetMyStoresAsync(int userId);
        Task<Store> GetMyStoreAsync(int storeId, int userId);

        // 建立 / 更新 / 刪除商店
        Task<int> CreateAsync(int userId, StoreInitRequest request);
        Task UpdateAsync(int userId, StoreUpdateRequest request);
        Task DeleteAsync(int userId, int storeId);

        // 圖片更新
        Task UpdateCoverImageAsync(int storeId, int ownerUserId, string coverImageUrl);
        Task UpdateMenuImageAsync(int storeId, int ownerUserId, string menuImageUrl);

        // ===== 休息日相關（唯一正確版本）=====
        Task<Store> GetMyStoreWithClosedDatesAsync(int storeId, int ownerUserId);

        Task AddClosedDateAsync(int storeId, int ownerUserId, DateTime closedDate);
        Task DeleteClosedDateAsync(int storeClosedDateId, int storeId, int ownerUserId);
    }
}

