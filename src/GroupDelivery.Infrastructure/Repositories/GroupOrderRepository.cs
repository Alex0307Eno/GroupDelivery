using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using GroupDelivery.Application.Abstractions;

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

        // 依照主鍵取得單筆開團資料
        public async Task<GroupOrder> GetByIdAsync(int id)
        {
            return await _db.GroupOrders
                .Include(g => g.Store)
                .FirstOrDefaultAsync(g => g.GroupOrderId == id);
        }

        // 取得所有進行中的開團資訊
        public async Task<List<GroupOrder>> GetAllActiveAsync()
        {
            return await _db.GroupOrders
                .Include(g => g.Store)
                .Where(g => g.Status == GroupOrderStatus.Open)
                .ToListAsync();
        }

        // 新增開團資料
        public async Task AddAsync(GroupOrder entity)
        {
            await _db.GroupOrders.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        // 更新開團資料
        public async Task UpdateAsync(GroupOrder entity)
        {
            _db.GroupOrders.Update(entity);
            await _db.SaveChangesAsync();
        }
        public async Task<GroupOrder> GetDetailAsync(int groupId)
        {
            return await _db.GroupOrders
                .Include(g => g.Store)
                .ThenInclude(s => s.MenuImageUrl)
                .FirstOrDefaultAsync(g => g.GroupOrderId == groupId);
        }

    }
}
