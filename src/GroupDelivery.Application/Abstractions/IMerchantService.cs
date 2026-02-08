using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Application.Abstractions
{
    public interface IMerchantService
    {
        // 將一般使用者升級為商家身分
        // 負責驗證申請資料、更新使用者角色與商家相關資訊
        // 僅處理商業流程，不直接操作資料庫
        Task UpgradeToMerchant(int userId, UpgradeMerchantRequest request);

        // 建立商家所屬的第一間商店
        // 通常在升級為商家後呼叫
        // 回傳新建立的商店 Id
        Task<int> CreateStoreAsync(int userId, MerchantInfoDto dto);
    }


}
