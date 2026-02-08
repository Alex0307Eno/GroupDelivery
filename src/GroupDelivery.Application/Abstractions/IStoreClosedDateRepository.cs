using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreClosedDateRepository
    {
        // 判斷指定商店在特定日期是否已設定為休息日
        // 用於避免重複新增相同日期的休息設定
        Task<bool> ExistsAsync(int storeId, DateTime closedDate);

        // 新增一筆指定日期的休息日資料
        // 僅負責資料寫入，不包含任何商業邏輯判斷
        Task AddAsync(StoreClosedDate entity);

        // 刪除指定的休息日設定
        // 通常用於商家手動取消休息日
        Task DeleteAsync(int storeClosedDateId);

        // 取得指定商店所有已設定的休息日清單
        // 供後台管理或營業狀態判斷使用
        Task<List<StoreClosedDate>> GetByStoreIdAsync(int storeId);
    }

}
