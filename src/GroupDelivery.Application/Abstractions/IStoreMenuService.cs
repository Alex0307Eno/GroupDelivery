using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuService
    {
        Task<List<StoreMenuItem>> GetMenuAsync(int storeId);
        Task ToggleActiveAsync(int menuItemId);
        Task BatchCreateAsync(int userId, int storeId, List<MenuItemDto> items);
        Task ReorderCategoriesAsync(int userId, List<CategoryReorderRequest> request);
        Task BatchSetCategoryActiveAsync(int userId, List<CategoryActiveRequest> request);
        Task<List<MenuItemAvailableDto>> GetAvailableItemsAsync(int userId, string time);
        Task TransferCategoryAsync(int userId, CategoryTransferRequest request);
    }
}
