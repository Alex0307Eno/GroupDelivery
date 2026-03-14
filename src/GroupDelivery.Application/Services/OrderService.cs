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
    // 訂單服務，負責訂單建立與查詢等商業邏輯
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

        #region Business Logic

        // 建立一般訂單，金額與選項加價一律由資料庫重新計算
        public async Task CreateOrderAsync(int userId, CreateOrderRequest request)
        {
            // 1. 檢查團
            var groupOrder = await _groupOrderRepository.GetByPublicIdAsync(request.GroupOrderPublicId);
            if (groupOrder == null)
                throw new Exception("揪團不存在");

            if (groupOrder.Deadline <= DateTime.Now)
                throw new Exception("揪團已截止");

            if (groupOrder.Status != GroupOrderStatus.Open)
                throw new Exception("揪團不可下單");

            var firstMenuStoreId = 0;

            // 2. 抓菜單
            var menuIds = request.Items.Select(x => x.StoreMenuItemPublicId).ToList();
            var menuItems = await _storeMenuItemRepository.GetByIdsAsync(menuIds);
            var user = await _userRepository.GetByIdAsync(userId);
            
            if (user == null)
                throw new Exception("使用者不存在");

            var order = new Order
            {
                OrderPublicId = Guid.NewGuid(),
                UserId = userId,
                GroupOrderId = groupOrder.GroupOrderId,
                ContactPhone = user.Phone,
                ContactName = user.DisplayName,
                Note = request.Note,
                CreatedAt = DateTime.Now,
                Source = OrderSource.Online,
                OrderItems = new List<OrderItem>()
            }; 
            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                if (item.Quantity <= 0)
                    throw new Exception("訂購數量錯誤");

                var menu = menuItems.FirstOrDefault(x => x.StoreMenuItemPublicId == item.StoreMenuItemPublicId);

                if (menu == null)
                    throw new Exception("菜單項目不存在");

                if (firstMenuStoreId == 0)
                    firstMenuStoreId = menu.StoreId;

                if (menu.StoreId != firstMenuStoreId || menu.StoreId != groupOrder.StoreId)
                    throw new Exception("菜單不屬於此團單店家");

                decimal optionTotal = 0;

                var orderItem = new OrderItem
                {
                    StoreMenuItemId = menu.StoreMenuItemId,
                    Quantity = item.Quantity,
                    UnitPrice = menu.Price,
                    OrderItemOptions = new List<OrderItemOption>()
                };

                // 依資料庫選項重新計算加價，避免信任前端傳入價格
                if (item.Options != null && item.Options.Any())
                {
                    var dbOptions = menu.OptionGroups != null
                        ? menu.OptionGroups.SelectMany(x => x.Options ?? new List<StoreMenuItemOption>()).ToList()
                        : new List<StoreMenuItemOption>();

                    foreach (var opt in item.Options)
                    {
                        var matchedOption = dbOptions.FirstOrDefault(x => x.OptionName == opt.OptionName);

                        if (matchedOption == null)
                            throw new Exception("選項不存在");

                        optionTotal += matchedOption.PriceAdjust;

                        orderItem.OrderItemOptions.Add(new OrderItemOption
                        {
                            OptionName = matchedOption.OptionName,
                            PriceAdjust = matchedOption.PriceAdjust
                        });
                    }
                }

                // 單價與小計由後端計算
                orderItem.UnitPrice = menu.Price + optionTotal;
                var subtotal = orderItem.UnitPrice * item.Quantity;

                totalAmount += subtotal;
                order.OrderItems.Add(orderItem);
            }
            order.TotalAmount = totalAmount;

            await _orderRepository.AddAsync(order);

            groupOrder.CurrentAmount += totalAmount;

            

            await _groupOrderRepository.UpdateAsync(groupOrder);
        }
        // 依團單查詢訂單
        public async Task<List<Order>> GetOrdersByGroupAsync(Guid groupOrderPublicId)
        {
            return await _orderRepository.GetByGroupPublicIdAsync(groupOrderPublicId);
        }

        // 建立人工訂單，僅允許團主操作
        public async Task CreateManualOrderAsync(int userId, CreateManualOrderRequest request)
        {
            var group = await _groupOrderRepository.GetByPublicIdAsync(request.GroupOrderPublicId);

            if (group == null)
                throw new Exception("團不存在");

            if (group.OwnerUserId != userId)
                throw new Exception("只有團主可以操作");

            if (group.Status != GroupOrderStatus.Open)
                throw new Exception("目前狀態不可操作");

            var order = new Order
            {
                OrderPublicId = Guid.NewGuid(),
                UserId = userId,
                GroupOrderId = group.GroupOrderId,
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
        // 取得個人訂單
        public async Task<List<Order>> GetMyOrdersAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
        // 取得商家訂單
        public async Task<List<Order>> GetOrdersForMerchantAsync(int merchantUserId)
        {
            return await _orderRepository.GetOrdersForMerchantAsync(merchantUserId);
        }

        #endregion
    }
}