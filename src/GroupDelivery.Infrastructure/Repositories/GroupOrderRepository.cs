using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    // 此類別為 GroupOrder 的資料存取物件
    // 負責讀取與寫入資料庫中的開團資訊
    public class GroupOrderRepository:IGroupOrderRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public GroupOrderRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        #region 依照主鍵取得單筆開團資料
        public async Task<GroupOrder> GetByIdAsync(int id)
        {
            return await _db.GroupOrders
                .Include(g => g.Store)
                .FirstOrDefaultAsync(g => g.GroupOrderId == id);
        }
        #endregion

        #region 取得所有進行中的開團資訊
        public async Task<List<GroupOrder>> GetAllActiveAsync()
        {
            return await _db.GroupOrders
                .Include(g => g.Store)
                .Where(g => g.Status == GroupOrderStatus.Open)
                .ToListAsync();
        }
        #endregion

        #region 新增開團資料
        public async Task AddAsync(GroupOrder entity)
        {
            await _db.GroupOrders.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
        #endregion

        #region 更新開團資料
                public async Task UpdateAsync(GroupOrder entity)
                {
                    _db.GroupOrders.Update(entity);
                    await _db.SaveChangesAsync();
                }
        #endregion

        #region 取得指定團單的詳細資料，供團單詳情頁顯示
        public async Task<GroupOrder> GetDetailAsync(int groupId)
        {
            return await _db.GroupOrders
                .Include(g => g.Store)
                .Include(g => g.GroupOrderItems) // 如果要算人數
                .FirstOrDefaultAsync(g => g.GroupOrderId == groupId);
        }
        #endregion

        #region 取得已超過截止時間但仍為進行中狀態的團單
        public async Task<List<GroupOrder>> GetActiveOverdueAsync(DateTime now)
        {
            return await _db.GroupOrders
                .Where(g =>
                    g.Status == GroupOrderStatus.Open &&
                    g.Deadline < now)
                .ToListAsync();
        }
        #endregion

        #region 取得指定使用者所建立的所有團單
        public async Task<List<GroupOrder>> GetByCreatorAsync(int creatorUserId)
        {

            var groups = await _db.GroupOrders
                .Include(g => g.Store)
                .Where(g => g.CreatorUserId == creatorUserId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();

            var now = DateTime.Now;
            bool changed = false;

            foreach (var g in groups)
            {
                // 如果還是 Open 但時間已過 → 改成 Expired
                if (g.Status == GroupOrderStatus.Open && g.Deadline < now)
                {
                    g.Status = GroupOrderStatus.Expired;
                    changed = true;
                }
            }
            if (changed)
            {
                await _db.SaveChangesAsync();
            }

            return groups;
        }

        #endregion

        #region 取得指定店家的所有團單
        public async Task<List<GroupOrder>> GetOpenGroupsAsync(DateTime now)
        {
            return await _db.GroupOrders
                .Where(g => g.Status == GroupOrderStatus.Open
                         && g.Deadline > now)
                .ToListAsync();
        }
        #endregion

        #region 新增一筆團單內的訂單項目資料
        public async Task AddItemAsync(GroupOrderItem item)
        {
            _db.GroupOrderItems.Add(item);
            await _db.SaveChangesAsync();
        }
        #endregion

        #region 取得指定店家目前所有仍在揪團中的團單
        public async Task<List<GroupOrder>> GetOpenByStoreAsync(int storeId, DateTime now)
        {
            return await _db.GroupOrders
                .Where(g => g.StoreId == storeId
                         && g.Status == GroupOrderStatus.Open
                         && g.Deadline > now)
                .OrderBy(g => g.Deadline)
                .ToListAsync();
        }
        #endregion

        public async Task<List<GroupOrder>> GetOpenGroupsWithStoreAsync()
        {
            var now = DateTime.Now;

            return await _db.GroupOrders
                .Include(x => x.Store)
                .Where(x => x.Status == GroupOrderStatus.Open
                            && x.Deadline > now)
                .ToListAsync();
        }

       

       
    }
}
