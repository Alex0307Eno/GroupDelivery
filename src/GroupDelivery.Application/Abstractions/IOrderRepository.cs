using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Application.Abstractions
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<List<Order>> GetByGroupOrderIdAsync(int groupOrderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<List<Order>> GetOrdersForMerchantAsync(int merchantUserId);
    }
}
