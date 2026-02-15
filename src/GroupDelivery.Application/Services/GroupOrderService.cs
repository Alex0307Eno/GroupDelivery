using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Exceptions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class GroupOrderService : IGroupOrderService
    {
        private readonly IGroupOrderRepository _groupOrderRepository;
        private readonly IUserRepository _userRepo;
        private readonly IStoreRepository _storeRepo;
        private readonly IStoreMenuRepository _storeMenuRepository;

        public GroupOrderService(
            IGroupOrderRepository groupOrderRepository,
            IUserRepository userRepository,
            IStoreRepository storeRepository,
            IStoreMenuRepository storeMenuRepository)
        {
            _groupOrderRepository = groupOrderRepository;
            _userRepo = userRepository;
            _storeRepo = storeRepository;
            _storeMenuRepository = storeMenuRepository;
        }

        public async Task<GroupDetailDto> GetGroupDetailAsync(int groupId)
        {
            var group = await _groupOrderRepository.GetDetailAsync(groupId);
            if (group == null)
                return null;

            return new GroupDetailDto
            {
                GroupId = group.GroupOrderId,
                StoreName = group.Store.StoreName,
                TargetAmount = group.TargetAmount,
                CurrentAmount = group.CurrentAmount,
                Deadline = group.Deadline,
                MenuImages = group.Store.MenuImageUrl?.Split(',').ToList()
            };
        }

        public async Task CreateGroupAsync(int userId, CreateGroupRequest request)
        {
            var store = await _storeRepo.GetByIdAsync(request.StoreId);
            if (store == null)
                throw new BusinessException("店家不存在");

            var isMerchant = await _userRepo.IsMerchantAsync(userId);
            if (isMerchant && store.OwnerUserId != userId)
                throw new BusinessException("無權限操作此店家");

            if (request.Deadline <= DateTime.Now)
                throw new BusinessException("截止時間必須晚於現在");

            var group = new GroupOrder
            {
                StoreId = request.StoreId,
                CreatorUserId = userId,
                OwnerUserId = store.OwnerUserId,
                TargetAmount = request.TargetAmount,
                CurrentAmount = 0,
                Deadline = request.Deadline,
                Status = GroupOrderStatus.Open,
                CreatedAt = DateTime.Now,
                Remark = request.Remark
            };

            await _groupOrderRepository.AddAsync(group);
        }

        public async Task<List<GroupOrder>> GetMyGroupOrdersAsync(int userId)
        {
            return await _groupOrderRepository.GetByCreatorAsync(userId);
        }

        public async Task JoinGroupAsync(int userId, int groupOrderId, decimal amount)
        {
            var group = await _groupOrderRepository.GetByIdAsync(groupOrderId);
            EnsureJoinable(group);

            var quantity = 1;
            var firstMenuItem = (await _storeMenuRepository.GetByStoreIdAsync(group.StoreId)).FirstOrDefault(x => x.IsActive);
            if (firstMenuItem == null)
                throw new BusinessException("目前無可用菜單品項");

            var menuItemName = firstMenuItem.Name;
            var unitPrice = firstMenuItem.Price > 0 ? firstMenuItem.Price : amount;

            var item = new GroupOrderItem
            {
                GroupOrderId = groupOrderId,
                UserId = userId,
                StoreMenuItemId = firstMenuItem.StoreMenuItemId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                SubTotal = unitPrice * quantity,
                MenuItemNameSnapshot = menuItemName,
                UnitPriceSnapshot = unitPrice,
                LineTotalSnapshot = unitPrice * quantity,
                CreatedAt = DateTime.Now
            };

            await _groupOrderRepository.AddItemAsync(item);

            group.CurrentAmount += item.LineTotalSnapshot;
            if (group.CurrentAmount >= group.TargetAmount)
            {
                group.Status = GroupOrderStatus.Success;
            }

            await _groupOrderRepository.UpdateAsync(group);
        }

        public async Task<List<GroupOrder>> GetOpenGroupsByStoreAsync(int storeId)
        {
            return await _groupOrderRepository.GetOpenByStoreAsync(storeId, DateTime.Now);
        }

        public async Task<GroupDetailDto> GetDetailAsync(int groupId)
        {
            var group = await _groupOrderRepository.GetByIdAsync(groupId);
            if (group == null)
                return null;

            return new GroupDetailDto
            {
                GroupOrderId = group.GroupOrderId,
                Deadline = group.Deadline,
                CurrentAmount = group.CurrentAmount,
                TargetAmount = group.TargetAmount
            };
        }

        public async Task JoinGroupAsync(int userId, int groupId)
        {
            await JoinGroupAsync(userId, groupId, 100);
        }

        private static void EnsureJoinable(GroupOrder group)
        {
            if (group == null)
                throw new BusinessException("團不存在");

            if (group.Status != GroupOrderStatus.Open)
                throw new BusinessException("此團無法加入");

            if (group.Deadline <= DateTime.Now)
                throw new BusinessException("已截止");
        }
    }
}
