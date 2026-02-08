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

    }

}
