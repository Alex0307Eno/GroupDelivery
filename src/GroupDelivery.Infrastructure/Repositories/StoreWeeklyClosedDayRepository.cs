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
    public class StoreWeeklyClosedDayRepository
     : IStoreWeeklyClosedDayRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreWeeklyClosedDayRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }
        #region 替換指定店家的每週休息日設定
        public async Task ReplaceAsync(int storeId, List<int> days)
        {
            var exists = _db.StoreWeeklyClosedDays
                .Where(x => x.StoreId == storeId);

            _db.StoreWeeklyClosedDays.RemoveRange(exists);

            foreach (var day in days.Distinct())
            {
                _db.StoreWeeklyClosedDays.Add(new StoreWeeklyClosedDay
                {
                    StoreId = storeId,
                    DayOfWeek = day,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync();
        }
        #endregion

        #region 取得指定店家的每週休息日設定
        public async Task<List<int>> GetDaysByStoreIdAsync(int storeId)
        {
            return await _db.StoreWeeklyClosedDays
                .Where(x => x.StoreId == storeId)
                .Select(x => x.DayOfWeek)
                .ToListAsync();
        }
        #endregion
    }

}
