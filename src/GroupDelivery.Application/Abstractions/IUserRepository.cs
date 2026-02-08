using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IUserRepository
    {
        // 依使用者 Id 非同步取得使用者資料
        // 用於一般查詢流程，避免阻塞執行緒
        Task<User> GetByIdAsync(int userId);

        // 依 Email 取得使用者
        // 若不存在則建立新使用者並回傳
        // 主要用於無密碼登入或首次登入流程
        Task<User> GetOrCreateByEmail(string email);

        // 依使用者 Id 同步取得使用者資料
        // 僅適用於已在非同步流程外的特殊情境
        // 使用時需注意避免阻塞或效能問題
        User GetById(int userId);

        // 同步更新使用者資料
        // 適用於已由上層統一控制交易或 SaveChanges 的情境
        void Update(User user);

        // 非同步更新使用者資料
        // 一般情況下建議優先使用此方法
        Task UpdateAsync(User user);
    }
}
