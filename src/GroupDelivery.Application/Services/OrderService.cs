// OrderService.cs
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GroupDelivery.Domain.Order;


namespace GroupDelivery.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGroupOrderRepository _groupOrderRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IStoreMenuItemRepository _storeMenuItemRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(
            IGroupOrderRepository groupOrderRepository,
            IOrderRepository orderRepository,
            IStoreMenuItemRepository storeMenuItemRepository,
            IUserRepository userRepository)
        {
            _groupOrderRepository = groupOrderRepository;
            _orderRepository = orderRepository;
            _storeMenuItemRepository = storeMenuItemRepository;
            _userRepository = userRepository;
        }

        public async Task CreateOrderAsync(int userId, CreateOrderRequest request)
        {
            // 1. 檢查團
            var group = await _groupOrderRepository.GetByIdAsync(request.GroupOrderId);
            if (group == null)
                throw new Exception("揪團不存在");

            if (group.Deadline <= DateTime.Now)
                throw new Exception("揪團已截止");

            if (group.Status != GroupOrderStatus.Open)
                throw new Exception("揪團不可下單");

            // 2. 抓菜單
            var menuIds = request.Items.Select(x => x.StoreMenuItemId).ToList();
            var menuItems = await _storeMenuItemRepository.GetByIdsAsync(menuIds);
            var user = await _userRepository.GetByIdAsync(userId);

            var order = new Order
            {
                UserId = userId,
                GroupOrderId = group.GroupOrderId,
                ContactPhone = user.Phone,
                ContactName = user.DisplayName,
                CreatedAt = DateTime.Now,
                Source = OrderSource.Online,
                OrderItems = new List<OrderItem>(),

            };

            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                var menu = menuItems.FirstOrDefault(x => x.StoreMenuItemId == item.StoreMenuItemId);
                if (menu == null)
                    throw new Exception("菜單項目不存在");

                var subtotal = menu.Price * item.Quantity;
                totalAmount += subtotal;

                order.OrderItems.Add(new OrderItem
                {
                    StoreMenuItemId = menu.StoreMenuItemId,
                    Quantity = item.Quantity,
                    UnitPrice = menu.Price
                });
            }

            order.TotalAmount = totalAmount;

            await _orderRepository.AddAsync(order);

            group.CurrentAmount += totalAmount;

            

            await _groupOrderRepository.UpdateAsync(group);
        }
        public async Task<List<Order>> GetOrdersByGroupAsync(int groupId)
        {
            return await _orderRepository.GetByGroupOrderIdAsync(groupId);
        }

        public async Task CreateManualOrderAsync(int userId, CreateManualOrderRequest request)
        {
            var group = await _groupOrderRepository.GetByIdAsync(request.GroupOrderId);

            if (group == null)
                throw new Exception("團不存在");

            if (group.OwnerUserId != userId)
                throw new Exception("只有團主可以操作");

            if (group.Status != GroupOrderStatus.Open)
                throw new Exception("目前狀態不可操作");

            var order = new Order
            {
                UserId = userId,
                GroupOrderId = request.GroupOrderId,
                TotalAmount = request.Amount,
                CreatedAt = DateTime.Now,
                Source = OrderSource.Manual
            };

            await _orderRepository.AddAsync(order);

            group.CurrentAmount += request.Amount;

            if (group.CurrentAmount >= group.TargetAmount)
                group.Status = GroupOrderStatus.Success;

            await _groupOrderRepository.UpdateAsync(group);
        }
        public async Task<List<Order>> GetMyOrdersAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
        public async Task<List<Order>> GetOrdersForMerchantAsync(int merchantUserId)
        {
            return await _orderRepository.GetOrdersForMerchantAsync(merchantUserId);
        }
    }
}