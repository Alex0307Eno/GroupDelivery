using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class StoreMenuRepository : IStoreMenuRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreMenuRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(StoreMenuItem item)
        {
            // 這裡只負責追蹤實體，實際寫入交給 SaveChangesAsync
            await _db.StoreMenuItems.AddAsync(item);
        }
        public async Task<StoreMenuItem> GetByPublicIdAsync(Guid publicId)
        {
            return await _db.StoreMenuItems
                .Include(x => x.Store)
                .Include(x => x.OptionGroups)
                .ThenInclude(x => x.Options)
                .FirstOrDefaultAsync(x => x.StoreMenuItemPublicId == publicId);
        }
        public async Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuItems
        .Where(x => x.StoreId == storeId)
        .Include(x => x.OptionGroups)
            .ThenInclude(g => g.Options)
        .OrderBy(x => x.StoreMenuItemId)
        .ToListAsync();
        }

        public async Task<StoreMenuItem> GetByIdAsync(int id)
        {
            return await _db.StoreMenuItems
                .Include(x => x.OptionGroups)
                    .ThenInclude(g => g.Options)
                .FirstOrDefaultAsync(x => x.StoreMenuItemId == id);
        }

        // 名稱要跟介面一致，不能叫 UpdateAsync
        public void Update(StoreMenuItem item)
        {
            _db.StoreMenuItems.Update(item);
        }

        public void Remove(StoreMenuItem item)
        {
            _db.StoreMenuItems.Remove(item);
        }

        public async Task<StoreMenuItem> GetWithOptionsAsync(int id)
        {
            return await _db.StoreMenuItems
                .Include(x => x.OptionGroups)
                    .ThenInclude(g => g.Options)
                .FirstOrDefaultAsync(x => x.StoreMenuItemId == id);
        }
        public async Task<List<StoreMenuItem>> GetByStoreIdWithOptionsAsync(
     int storeId,
     bool includeInactive = false)
        {
            var query = _db.StoreMenuItems
                .Include(m => m.Category)
                .Include(m => m.OptionGroups)
                    .ThenInclude(g => g.Options)
                .Where(m => m.StoreId == storeId);

            if (!includeInactive)
                query = query.Where(m => m.IsActive);

            return await query
                .OrderBy(m => m.StoreMenuItemId)
                .ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
        #region 判斷是否有菜單
        public async Task<bool> AnyActiveByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuItems
                .AnyAsync(x => x.StoreId == storeId && x.IsActive);
        }
        #endregion

        public async Task<bool> AnyByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuItems.AnyAsync(x => x.StoreId == storeId);
        }
    }
}