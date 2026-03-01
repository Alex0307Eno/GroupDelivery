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

        public async Task<List<Order>> GetByGroupOrderIdAsync(int groupOrderId)
        {
            return await _db.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.User) 
                .Where(x => x.GroupOrderId == groupOrderId)
                .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _db.Orders
                .Include(x => x.GroupOrder)
                    .ThenInclude(g => g.Store)
                .Include(x => x.OrderItems)
                    .ThenInclude(i => i.StoreMenuItem)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersForMerchantAsync(int merchantUserId)
        {
            return await _db.Orders
                .Include(x => x.User)
                .Include(x => x.GroupOrder)
                    .ThenInclude(g => g.Store)
                .Include(x => x.OrderItems)
                    .ThenInclude(i => i.StoreMenuItem)
                .Include(x => x.OrderItems)
                    .ThenInclude(i => i.OrderItemOptions)  
                .Where(x => x.GroupOrder.Store.OwnerUserId == merchantUserId)
                .OrderByDescending(x => x.CreatedAt)
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