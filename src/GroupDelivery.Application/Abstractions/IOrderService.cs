using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IOrderService
    {
        Task CreateOrderAsync(int userId, CreateOrderRequest request);

        Task CreateManualOrderAsync(int userId, CreateManualOrderRequest request);

        Task<List<Order>> GetOrdersByGroupAsync(int groupId);
        Task<List<Order>> GetMyOrdersAsync(int userId);
        Task<List<Order>> GetOrdersForMerchantAsync(int merchantUserId);
    }
}
