using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuItems
                .Include(x => x.Category)   
                .Where(x => x.StoreId == storeId)
                .OrderBy(x => x.StoreMenuItemId)
                .ToListAsync();
        }

        public async Task<StoreMenuItem> GetByIdAsync(int id)
        {
            return await _db.StoreMenuItems
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

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}