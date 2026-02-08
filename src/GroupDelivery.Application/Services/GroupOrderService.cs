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
            if (request == null)
                throw new Exception("請求資料為空");

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("使用者不存在");

            if (user.Role != UserRole.Merchant)
                throw new Exception("只有商家可以開團");

            var store = await _storeRepo.GetFirstByOwnerAsync(userId);
            if (store == null)
                throw new Exception("找不到商家店家資料");

            var now = DateTime.Now;

            var group = new GroupOrder
            {
                StoreId = store.StoreId,
                CreatorUserId = userId,
                TargetAmount = request.TargetAmount,
                Deadline = request.Deadline,
                Remark = request.Remark,
                CreatedAt = now,
                CurrentAmount = 0,
                Status = GroupOrderStatus.Open
            };

            if (group.TargetAmount <= 0)
                throw new Exception("成團金額不正確");

            if (group.Deadline <= now.AddMinutes(30))
                throw new Exception("截止時間需至少 30 分鐘後");

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


    }
}
