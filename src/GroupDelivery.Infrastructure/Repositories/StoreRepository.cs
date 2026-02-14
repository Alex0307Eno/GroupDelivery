using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using GroupDelivery.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }
        #region 取得指定店家資料
        public async Task<Store> GetByIdAndOwnerAsync(int storeId, int ownerUserId)
        {
            return await _db.Stores
                .FirstOrDefaultAsync(x =>
                    x.StoreId == storeId &&
                    x.OwnerUserId == ownerUserId);
        }
        #endregion

        #region 取得指定使用者的所有店家資料
        public async Task<List<Store>> GetByOwnerAsync(int ownerUserId)
        {
            return await _db.Stores
                .Where(x => x.OwnerUserId == ownerUserId)
                .ToListAsync();
        }
        #endregion

        #region 取得指定使用者的第一間店家資料
        public async Task<Store> GetFirstByOwnerAsync(int ownerUserId)
        {
            return await _db.Stores
                .Where(x => x.OwnerUserId == ownerUserId)
                .OrderBy(x => x.StoreId)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region 商家CRUD
        public async Task<int> CreateAsync(Store store)
        {
            await _db.Stores.AddAsync(store);
            await _db.SaveChangesAsync();
            return store.StoreId;
        }

        public async Task UpdateAsync(Store store)
        {
            _db.Stores.Update(store);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Store store)
        {
            _db.Stores.Remove(store);
            await _db.SaveChangesAsync();
        }
        #endregion
        public async Task<Store> GetByIdAsync(int storeId)
        {
            return await _db.Stores
                .FirstOrDefaultAsync(s => s.StoreId == storeId);
        }
        public async Task<List<Store>> GetAllAsync()
        {
            return await _db.Stores.ToListAsync();
        }
    }
}
