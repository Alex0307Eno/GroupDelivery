using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuService
    {
        Task CreateMenuItemAsync(int userId, int storeId, string name, decimal price, string description);
        Task<List<StoreMenuItem>> GetMenuAsync(int storeId);
        Task ToggleActiveAsync(int menuItemId);
        Task BatchCreateAsync(int userId,int storeId,List<MenuItemDto> items);

    }

}
