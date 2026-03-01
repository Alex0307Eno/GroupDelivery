using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IGroupOrderService
    {
        

        // 建立一筆新的揪團
        // 負責所有建立前的商業邏輯驗證（身分、商店狀態、參數合法性）
        // Repository 僅負責資料寫入
        Task CreateGroupAsync(int userId, CreateGroupRequest request);

        // 取得指定團單的完整顯示資料
        // 會整合多個資料來源並轉換為 DTO
        // 供團單詳情頁使用，避免 Controller 直接操作 Domain Model
        Task<GroupDetailDto> GetGroupDetailAsync(int groupId);
        //取得我的揪團列表
        Task<List<GroupOrder>> GetMyGroupOrdersAsync(int userId);
        // 加入指定的團單
        Task JoinGroupAsync(int userId, int groupOrderId, decimal amount);
        
        Task<List<GroupOrder>> GetOpenGroupsByStoreAsync(int storeId);

        Task<GroupDetailDto> GetDetailAsync(int groupId);

        Task JoinGroupAsync(int userId, int groupId);
        Task<List<GroupSummaryDto>> GetOpenGroupsAsync(double? lat, double? lng);
        Task<GroupMenuDto> GetMenuAsync(int groupOrderId);
        Task<GroupOrder> GetByIdAsync(int id);
        Task UpdateAsync(GroupOrder groupOrder);
        Task SetTakeModeAsync(int orderId, TakeMode takeMode);
        Task<int> CreateAsync(CreateUserGroupRequest request, int userId);

    }

}
