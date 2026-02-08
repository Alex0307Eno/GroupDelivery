using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IAuthService
    {
        // 發送登入用的一次性驗證連結至指定 Email
        // 用於無密碼登入流程（Magic Link）
        Task SendLoginLinkAsync(string email);

        // 透過驗證 Token 完成登入流程
        // 負責驗證 Token、建立 Claims、寫入登入 Cookie
        Task SignInByTokenAsync(string token, HttpContext httpContext);

        // 判斷目前登入使用者是否已完成必要的個人資料設定
        // 通常用於登入後導向 Onboarding 或首頁的判斷
        bool IsProfileCompleted(ClaimsPrincipal user);

        // 重新簽發登入 Cookie
        // 用於使用者資料或角色異動後，刷新 Claims 狀態
        Task RefreshSignInAsync(int userId);
    }

}
