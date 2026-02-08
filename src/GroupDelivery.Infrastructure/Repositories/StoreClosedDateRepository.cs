using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class StoreClosedDateRepository : IStoreClosedDateRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreClosedDateRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public List<StoreClosedDate> GetByStoreId(int storeId)
        {
            return _db.StoreClosedDates
                      .Where(x => x.StoreId == storeId)
                      .ToList();
        }
        public async Task<List<StoreClosedDate>> GetByStoreIdAsync(int storeId)
        {
            return await _db.StoreClosedDates
                .Where(x => x.StoreId == storeId)
                .ToListAsync();
        }
        public async Task<bool> ExistsAsync(int storeId, DateTime closedDate)
        {
            return await _db.StoreClosedDates.AnyAsync(x =>
                x.StoreId == storeId &&
                x.ClosedDate.Date == closedDate.Date);
        }

        public async Task AddAsync(StoreClosedDate entity)
        {
            _db.StoreClosedDates.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int storeClosedDateId)
        {
            var entity = await _db.StoreClosedDates.FindAsync(storeClosedDateId);
            if (entity == null) return;

            _db.StoreClosedDates.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

}
