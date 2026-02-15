using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuRepository
    {
        Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId);
        Task<List<StoreMenuItem>> GetAllByStoreIdAsync(int storeId);
        Task<List<StoreMenuItem>> GetAvailableMenuItemsAsync(int storeId, TimeSpan atTime);
        Task<StoreMenuItem> GetByIdAsync(int id);
        Task AddAsync(StoreMenuItem item);
        Task UpdateAsync(StoreMenuItem item);
        Task SaveChangesAsync();

        Task<List<StoreMenuCategory>> GetCategoriesByStoreAsync(int storeId, bool? isActive);
        Task<List<StoreMenuCategory>> GetCategoriesByIdsAsync(int storeId, List<int> categoryIds);
        Task<StoreMenuCategory> GetCategoryByIdAsync(int categoryId);
        Task<bool> CategoryNameExistsAsync(int storeId, string name);
        Task AddCategoryAsync(StoreMenuCategory category);
        Task<bool> ExistsMenuItemByCategoryAsync(int categoryId);
        void RemoveCategory(StoreMenuCategory category);

        Task ReorderCategoriesAsync(int storeId, List<CategoryReorderRequest> requests);
        Task ToggleCategoryActiveAsync(int storeId, List<CategoryActiveUpdateRequest> requests);
        Task<int> TransferCategoryItemsAsync(int storeId, int sourceCategoryId, int targetCategoryId);
    }
}
