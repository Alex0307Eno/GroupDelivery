// OrderRepository.cs
using System;
using System.Threading.Tasks;
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using GroupDelivery.Domain;

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
    }
}