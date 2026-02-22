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
    public class StoreMenuItemRepository : IStoreMenuItemRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreMenuItemRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task<List<StoreMenuItem>> GetByIdsAsync(List<int> ids)
        {
            return await _db.StoreMenuItems
                .Where(x => ids.Contains(x.StoreMenuItemId))
                .ToListAsync();
        }
    }
}
