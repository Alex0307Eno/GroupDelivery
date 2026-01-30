using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    // 此服務類別用於處理開團相關業務邏輯
    public class GroupOrderService
    {
        private readonly IGroupOrderRepository _groupOrderRepo;

        public GroupOrderService(IGroupOrderRepository groupOrderRepo)
        {
            _groupOrderRepo = groupOrderRepo;
        }

        // 取得所有進行中的團購
        public async Task<List<GroupOrder>> GetActiveGroupsAsync()
        {
            return await _groupOrderRepo.GetAllActiveAsync();
        }

        // 取得團購詳情
        public async Task<GroupOrder> GetGroupDetailAsync(int id)
        {
            return await _groupOrderRepo.GetByIdAsync(id);
        }

        // 建立新團購
        public async Task CreateGroupAsync(GroupOrder group)
        {
            group.CreatedAt = DateTime.Now;
            group.CurrentAmount = 0;
            group.Status = "Active";

            await _groupOrderRepo.AddAsync(group);
        }

        // 更新團購進度與狀態
        public async Task RefreshGroupStatusAsync(int groupId)
        {
            var group = await _groupOrderRepo.GetByIdAsync(groupId);
            if (group == null)
            {
                return;
            }

            if (group.CurrentAmount >= group.TargetAmount && group.Status == "Active")
            {
                group.Status = "Success";
                await _groupOrderRepo.UpdateAsync(group);
            }
        }
    }
}
