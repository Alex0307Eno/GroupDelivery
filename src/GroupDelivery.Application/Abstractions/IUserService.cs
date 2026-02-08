using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IUserService
    {
        // 取得指定使用者的個人資料
        // 回傳 DTO 以隔離 Domain Model，避免前台直接操作 User 實體
        // 供個人資料頁或帳號設定頁使用
        Task<UserProfileDto> GetProfileAsync(int userId);

        // 更新使用者的個人資料
        // 僅允許更新可編輯欄位，避免影響身分、角色等核心資訊
        Task UpdateProfileAsync(int userId, UpdateProfileRequest req);
    }
}
