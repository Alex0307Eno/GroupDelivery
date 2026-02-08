using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreWeeklyClosedDayRepository
    {
        // 取得指定商店設定的每週固定休息日
        // 回傳的整數代表星期（定義由系統統一規範）
        Task<List<int>> GetDaysByStoreIdAsync(int storeId);

        // 取代指定商店的每週固定休息日設定
        // 會先清空既有設定，再寫入新的休息日清單
        // 用於整批更新，而非單筆新增
        Task ReplaceAsync(int storeId, List<int> days);
    }

}
