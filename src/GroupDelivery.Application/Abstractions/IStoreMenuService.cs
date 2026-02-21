using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuService
    {
        Task<List<StoreMenuItem>> GetMenuAsync(int storeId);

        Task ToggleActiveAsync(int userId, int id);

        Task BatchCreateAsync(int userId, int storeId, List<MenuItemDto> items);

        // 改成 async 版本，名稱跟實作對齊
        Task UpdateAsync(int userId, MenuItemEditDto dto);

        Task<MenuItemEditDto> GetForEditAsync(int userId, int id);

        // 刪除也要 async，不要用 void
        Task DeleteAsync(int userId, int id);
    }
}