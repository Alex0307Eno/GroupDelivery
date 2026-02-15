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
        Task<List<StoreMenuItem>> GetMenuAsync(int storeId);
        Task ToggleActiveAsync(int menuItemId);
        Task BatchCreateAsync(int userId,int storeId,List<MenuItemDto> items);

    }

}
