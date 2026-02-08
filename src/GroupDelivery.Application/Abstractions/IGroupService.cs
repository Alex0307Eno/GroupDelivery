using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDelivery.Domain;


namespace GroupDelivery.Application.Abstractions
{
    public interface IGroupService
    {
        // 依團單 Id 取得團單基本資料
        // 主要供後台管理或權限判斷使用
        // 不負責組合前台顯示資料
        Group GetById(int groupId);

        // 判斷指定使用者是否為該團單的建立者（團主）
        // 用於存取控制，例如管理頁、編輯、關團等操作
        bool IsOwner(int groupId, int userId);

        // 取得指定使用者所建立的所有團單
        // 通常用於「我的團單 / 團主管理」列表頁
        List<Group> GetMyGroups(int ownerUserId);

        // 取得目前團單的實際參與人數
        // 用於顯示成團進度或判斷是否已達成門檻
        int GetCurrentMemberCount(int groupId);

        // 取得團單設定的目標成團人數
        // 與目前人數搭配，用於顯示進度條或成團條件
        int GetTargetMemberCount(int groupId);
        //結束/取消團單
        Task CloseAsync(int groupId, int userId);
        Task CancelAsync(int groupId, int userId);

        Task<GroupOrder> GetByIdAsync(int groupId);



    }

}
