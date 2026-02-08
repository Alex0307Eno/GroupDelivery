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
        #region 取得指定店家的休息日期列表
        public List<StoreClosedDate> GetByStoreId(int storeId)
        {
            return _db.StoreClosedDates
                      .Where(x => x.StoreId == storeId)
                      .ToList();
        }
        #endregion

        #region 非同步版本的取得指定店家的休息日期列表
        public async Task<List<StoreClosedDate>> GetByStoreIdAsync(int storeId)
        {
            return await _db.StoreClosedDates
                .Where(x => x.StoreId == storeId)
                .ToListAsync();
        }
        #endregion

        #region 判斷指定商店在特定日期是否已設定為休息日
        public async Task<bool> ExistsAsync(int storeId, DateTime closedDate)
        {
            return await _db.StoreClosedDates.AnyAsync(x =>
                x.StoreId == storeId &&
                x.ClosedDate.Date == closedDate.Date);
        }
        #endregion

        #region 新增一筆指定日期的休息日資料
        public async Task AddAsync(StoreClosedDate entity)
        {
            _db.StoreClosedDates.Add(entity);
            await _db.SaveChangesAsync();
        }
        #endregion

        #region 刪除指定的休息日設定
        public async Task DeleteAsync(int storeClosedDateId)
        {
            var entity = await _db.StoreClosedDates.FindAsync(storeClosedDateId);
            if (entity == null) return;

            _db.StoreClosedDates.Remove(entity);
            await _db.SaveChangesAsync();
        }
        #endregion
    }

}
