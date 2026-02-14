using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace GroupDelivery.Application.Services
{
    // 此服務類別用於處理開團相關業務邏輯
    public class GroupOrderService: IGroupOrderService
    {
        private readonly IGroupOrderRepository _groupOrderRepository;
        private readonly IUserRepository _userRepo;
        private readonly IStoreRepository _storeRepo;

        public GroupOrderService(IGroupOrderRepository groupOrderRepository, IUserRepository userRepository, IStoreRepository storeRepository)
        {
            _groupOrderRepository = groupOrderRepository; ;
            _userRepo = userRepository;
            _storeRepo = storeRepository;
        }
        #region 取得指定團單的詳細資料，供團單詳情頁顯示
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
                MenuImages = group.Store.MenuImageUrl ?.Split(',').ToList()
            };
        }
        #endregion

        #region 建立一筆新的揪團，僅允許商家開團
        public async Task CreateGroupAsync(int userId, CreateGroupRequest request)
        {
            // 檢查店是否存在
            var store = await _storeRepo.GetByIdAsync(request.StoreId);

            if (store == null)
                throw new Exception("店家不存在");

            // 如果是商家角色，驗證擁有權
            var isMerchant = await _userRepo.IsMerchantAsync(userId);

            if (isMerchant)
            {
                if (store.OwnerUserId != userId)
                    throw new Exception("無權限操作此店家");
            }

            var group = new GroupOrder
            {
                StoreId = request.StoreId,
                CreatorUserId = userId,
                TargetAmount = request.TargetAmount,
                Deadline = request.Deadline,
                Status = GroupOrderStatus.Open,
                CreatedAt = DateTime.Now
            };

            await _groupOrderRepository.AddAsync(group);
        }
        #endregion

        #region 將已超過截止時間的揪團標記為過期狀態
        public async Task ExpireOverdueGroupsAsync()
        {
            var now = DateTime.UtcNow;

            var overdueGroups = await _groupOrderRepository
                .GetActiveOverdueAsync(now);

            foreach (var group in overdueGroups)
            {
                group.Status = GroupOrderStatus.Expired;
            }

        }
        #endregion
        #region 取得我開的團列表
        public async Task<List<GroupOrder>> GetMyGroupOrdersAsync(int userId)
        {
            return await _groupOrderRepository.GetByCreatorAsync(userId);
        }
        #endregion
        #region 加入團單，新增訂單明細並更新團金額，判斷是否成團
        public async Task JoinGroupAsync(int userId, int groupOrderId, decimal amount)
        {
            var group = await _groupOrderRepository.GetByIdAsync(groupOrderId);

            if (group == null)
                throw new Exception("團不存在");

            if (group.Status != GroupOrderStatus.Open)
                throw new Exception("此團無法加入");

            if (group.Deadline <= DateTime.Now)
                throw new Exception("已截止");

            // 新增訂單明細
            var item = new GroupOrderItem
            {
                GroupOrderId = groupOrderId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            await _groupOrderRepository.AddItemAsync(item);

            // 更新團金額
            group.CurrentAmount += amount;

            // 判斷是否成團
            if (group.CurrentAmount >= group.TargetAmount)
            {
                group.Status = GroupOrderStatus.Success;
            }

            await _groupOrderRepository.UpdateAsync(group);
        }
        #endregion
        #region 取得指定商店的所有開團中團單
        public async Task<List<GroupOrder>> GetOpenGroupsByStoreAsync(int storeId)
        {
            var now = DateTime.Now;

            return await _groupOrderRepository
                .GetOpenByStoreAsync(storeId, now);
        }
        #endregion
        public async Task<GroupDetailDto> GetDetailAsync(int groupId)
        {
            var group = await _groupOrderRepository.GetByIdAsync(groupId);

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
            var group = await _groupOrderRepository.GetByIdAsync(groupId);

            if (group.Status != GroupOrderStatus.Open)
                throw new Exception("團已結束");

            group.CurrentAmount += 100; // 暫時固定金額

            await _groupOrderRepository.UpdateAsync(group);
        }

    }
}
