using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class GroupRepository:IGroupRepository
    {
        private readonly GroupDeliveryDbContext _db;
        public GroupRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }


        #region 取得指定使用者的所有群單資料
        public async Task<List<GroupOrder>> GetByOwnerAsync(int ownerUserId)
        {
            return await _db.GroupOrders
                .Where(g => g.CreatorUserId == ownerUserId)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        #endregion
        #region 取得指定群單的詳細資料，包含店家資訊
        public async Task<GroupOrder> GetByIdAsync(int groupId)
        {
            return await _db.GroupOrders
                .Include(g => g.Store)
                .FirstOrDefaultAsync(g => g.GroupOrderId == groupId);
        }
        #endregion
        #region 建立一筆新的群單資料
        public async Task UpdateAsync(GroupOrder group)
        {
            _db.GroupOrders.Update(group);
            await _db.SaveChangesAsync();
        }
        #endregion
    }
}
