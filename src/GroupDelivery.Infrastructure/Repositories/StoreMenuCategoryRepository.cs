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
    public class StoreMenuCategoryRepository
     : IStoreMenuCategoryRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreMenuCategoryRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(StoreMenuCategory entity)
        {
            _db.StoreMenuCategories.Add(entity);
            await Task.CompletedTask;
        }

        public async Task<List<StoreMenuCategory>> GetByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuCategories
                .Where(x => x.StoreId == storeId)
                .OrderBy(x => x.StoreMenuCategoryId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }

}
