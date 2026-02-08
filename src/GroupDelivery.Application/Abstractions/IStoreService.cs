using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreService
    {
        // 取得目前登入使用者所擁有的所有商店
        // 用於「我的商店」列表頁
        Task<List<Store>> GetMyStoresAsync(int userId);

        // 取得指定使用者名下的單一商店
        // 會同時進行擁有權驗證，避免越權存取
        Task<Store> GetMyStoreAsync(int storeId, int userId);

        // =========================
        // 商店建立 / 更新 / 刪除
        // =========================

        // 建立一間新商店
        // 僅允許由商家身分的使用者建立
        // 回傳新建立的商店 Id
        Task<int> CreateAsync(int userId, StoreInitRequest request);

        // 更新商店基本資料
        // 包含營業資訊與顯示用資料
        Task UpdateAsync(int userId, StoreUpdateRequest request);

        // 刪除指定商店
        // 僅允許商店擁有者操作
        Task DeleteAsync(int userId, int storeId);

        // =========================
        // 商店圖片管理
        // =========================

        // 更新商店封面圖片
        // 僅負責資料更新，不處理檔案實體儲存
        Task UpdateCoverImageAsync(
            int storeId,
            int ownerUserId,
            string coverImageUrl);

        // 更新商店菜單圖片
        // 目前以單張圖片為最小可用版本
        Task UpdateMenuImageAsync(
            int storeId,
            int ownerUserId,
            string menuImageUrl);

        // =========================
        // 商店休息日設定（營業狀態核心）
        // =========================

        // 取得包含休息日設定的商店完整資料
        // 供商家後台管理畫面使用
        Task<Store> GetMyStoreWithClosedDatesAsync(
            int storeId,
            int ownerUserId);

        // 新增指定日期的休息日
        // 用於臨時公休、國定假日等情境
        Task AddClosedDateAsync(
            int storeId,
            int ownerUserId,
            DateTime closedDate);

        // 刪除指定的休息日設定
        // 僅允許商店擁有者操作
        Task DeleteClosedDateAsync(
            int storeClosedDateId,
            int storeId,
            int ownerUserId);

        // 更新每週固定休息日設定
        // days 代表星期（例如 0~6 或 1~7，由實作層統一定義）
        // 用於設定例行性公休日
        Task UpdateWeeklyClosedDaysAsync(
            int storeId,
            int ownerUserId,
            List<int> days);
    }
}

