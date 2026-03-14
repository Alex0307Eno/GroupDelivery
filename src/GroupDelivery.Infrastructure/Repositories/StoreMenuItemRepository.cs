using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;    

namespace GroupDelivery.Infrastructure.Repositories
{
    // 菜單品項資料存取實作，僅負責資料庫查詢
    public class StoreMenuItemRepository : IStoreMenuItemRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreMenuItemRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        #region Database Access

        // 依菜單品項識別碼清單查詢，包含客製化選項資料
        public async Task<List<StoreMenuItem>> GetByIdsAsync(List<int> ids)
        {
            return await _db.StoreMenuItems
                .Where(x => ids.Contains(x.StoreMenuItemId))
                .Include(x => x.OptionGroups)
                .ThenInclude(g => g.Options)
                .ToListAsync();
        }

        #endregion
    }
}
