// OrderRepository.cs
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public OrderRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Order>> GetByGroupPublicIdAsync(Guid groupPublicId)
        {
            var groupOrder = await _db.GroupOrders
                .Where(x => x.GroupOrderPublicId == groupPublicId)
                .Select(x => new { x.GroupOrderId })
                .FirstOrDefaultAsync();

            if (groupOrder == null)
                return new List<Order>();

            return await _db.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.User)
                .Where(x => x.GroupOrderId == groupOrder.GroupOrderId)
                .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _db.Orders
                .Include(x => x.GroupOrder)
                    .ThenInclude(g => g.Store)
                .Include(x => x.OrderItems)
                    .ThenInclude(i => i.StoreMenuItem)
                .Include(x => x.OrderItems)
                    .ThenInclude(i => i.OrderItemOptions)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersForMerchantAsync(int merchantUserId)
        {
            return await _db.Orders
                .Include(o => o.User)

                .Include(o => o.GroupOrder)
                    .ThenInclude(g => g.Store)

                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.StoreMenuItem)

                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.OrderItemOptions)

                .Where(o =>
                    o.GroupOrder != null &&
                    o.GroupOrder.Store.OwnerUserId == merchantUserId)

                .ToListAsync();
        }
        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _db.Orders
                .FirstOrDefaultAsync(x => x.OrderId == orderId);
        }
        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
        }
    }
}