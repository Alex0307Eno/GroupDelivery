using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuRepository
    {
        Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId);
        Task<StoreMenuItem> GetByIdAsync(int id);
        Task AddAsync(StoreMenuItem item);
        Task UpdateAsync(StoreMenuItem item);
        Task SaveChangesAsync();
        Task<List<StoreMenuCategory>> GetCategoriesByIdsAsync(List<int> categoryIds);
        Task ReorderCategoriesAsync(List<CategoryReorderRequest> request);
        Task BatchUpdateCategoryActiveAsync(List<CategoryActiveRequest> request);
        Task TransferCategoryAsync(int sourceCategoryId, int targetCategoryId);
        Task<List<StoreMenuItem>> GetAvailableItemsAsync(int storeId, TimeSpan time);
    }
}
