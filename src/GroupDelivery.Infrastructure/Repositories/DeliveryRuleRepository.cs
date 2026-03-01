using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class DeliveryRuleRepository : IDeliveryRuleRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public DeliveryRuleRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task<List<DeliveryRule>> GetByStoreIdAsync(int storeId)
        {
            return await _db.DeliveryRules
                .Where(x => x.StoreId == storeId)
                .OrderBy(x => x.MaxDistanceKm)
                .ToListAsync();
        }

        public async Task AddAsync(DeliveryRule rule)
        {
            _db.DeliveryRules.Add(rule);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.DeliveryRules.FindAsync(id);
            if (entity != null)
            {
                _db.DeliveryRules.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}