// OrderService.cs
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace GroupDelivery.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGroupOrderRepository _groupOrderRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IStoreMenuItemRepository _storeMenuItemRepository;

        public OrderService(
            IGroupOrderRepository groupOrderRepository,
            IOrderRepository orderRepository,
            IStoreMenuItemRepository storeMenuItemRepository)
        {
            _groupOrderRepository = groupOrderRepository;
            _orderRepository = orderRepository;
            _storeMenuItemRepository = storeMenuItemRepository;
        }

        public async Task CreateOrderAsync(int userId, CreateOrderRequest request)
        {
            // 1. 檢查團是否存在
            var group = await _groupOrderRepository.GetByIdAsync(request.GroupOrderId);
            if (group == null)
                throw new Exception("揪團不存在");

            // 2. 檢查是否截止
            if (group.Deadline <= DateTime.Now)
                throw new Exception("揪團已截止");

            // 3. 檢查狀態
            if (group.Status != GroupOrderStatus.Open)
                throw new Exception("揪團不可下單");

            // 4. 重新抓菜單價格
            var menuIds = request.Items.Select(x => x.StoreMenuItemId).ToList();
            var menuItems = await _storeMenuItemRepository.GetByIdsAsync(menuIds);

            decimal totalAmount = 0;

            foreach (var item in request.Items)
            {
                var menu = menuItems.FirstOrDefault(x => x.StoreMenuItemId == item.StoreMenuItemId);
                if (menu == null)
                    throw new Exception("菜單項目不存在");

                totalAmount += menu.Price * item.Quantity;
            }

            // 5. 建立訂單
            var order = new Order
            {
                UserId = userId,
                GroupOrderId = group.GroupOrderId,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.Now
            };

            await _orderRepository.AddAsync(order);

            // 6. 更新團金額
            group.CurrentAmount += totalAmount;

            // 7. 判斷是否成團
            if (group.CurrentAmount >= group.TargetAmount)
            {
                group.Status = GroupOrderStatus.Success;
            }

            await _groupOrderRepository.UpdateAsync(group);
        }
    }
}