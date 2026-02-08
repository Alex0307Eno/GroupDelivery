using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IGroupOrderRepository
    {
        // 取得目前仍應顯示在前台的有效團單
        // 一般只包含「揪團中」狀態，排除已截止、取消、歷史團
        Task<List<GroupOrder>> GetAllActiveAsync();

        // 依團單 Id 取得單筆團單基本資料
        // 不保證包含關聯資料（Items、Store 等）
        Task<GroupOrder> GetByIdAsync(int id);

        // 取得團單完整明細資料
        // 通常會包含關聯資料（例如團內訂單、商店資訊）
        Task<GroupOrder> GetDetailAsync(int groupId);

        // 新增一筆團單
        // 僅負責資料持久化，不處理任何商業邏輯
        Task AddAsync(GroupOrder group);

        // 更新既有團單資料
        // 由 Service 層決定哪些欄位可被修改
        Task UpdateAsync(GroupOrder group);

        // 取得「已超過截止時間但尚未處理狀態」的團單清單
        // 通常用於背景排程，將團單狀態更新為 Closed
        Task<List<GroupOrder>> GetActiveOverdueAsync(DateTime now);
    }
}
