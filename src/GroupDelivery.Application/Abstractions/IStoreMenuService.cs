using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuService
    {
        Task<List<StoreMenuItem>> GetMenuAsync(int storeId);
        Task<List<StoreMenuItem>> GetAvailableMenuItemsAsync(int storeId, TimeSpan? atTime);
        Task<List<StoreMenuCategory>> GetCategoriesAsync(int storeId, bool? isActive);
        Task ToggleActiveAsync(int menuItemId);
        Task BatchCreateAsync(int storeId, List<MenuItemDto> items);
        Task CreateCategoryAsync(int storeId, string name, int sortOrder);
        Task DeleteCategoryAsync(int storeId, int categoryId);
        Task ReorderCategoriesAsync(int storeId, List<CategoryReorderRequest> requests);
        Task ToggleCategoryActiveAsync(int storeId, List<CategoryActiveUpdateRequest> requests);
        Task TransferCategoryItemsAsync(int storeId, CategoryTransferRequest request);
    }
}
