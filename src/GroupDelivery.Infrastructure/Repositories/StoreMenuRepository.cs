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
    public class StoreMenuRepository : IStoreMenuRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreMenuRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(StoreMenuItem item)
        {
            _db.StoreMenuItems.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuItems
                .Where(x => x.StoreId == storeId && x.IsActive)
                .ToListAsync();
        }

        public async Task<StoreMenuItem> GetByIdAsync(int id)
        {
            return await _db.StoreMenuItems
                .FirstOrDefaultAsync(x => x.StoreMenuItemId == id);
        }

        public async Task UpdateAsync(StoreMenuItem item)
        {
            _db.StoreMenuItems.Update(item);
            await _db.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
