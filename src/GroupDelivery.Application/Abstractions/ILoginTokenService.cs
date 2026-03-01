using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface ILoginTokenService
    {
        // 產生一次性登入用的 Token
        // 通常會與 Email 綁定，並具備時效性
        // 用於無密碼登入（Magic Link）流程
        string GenerateToken(string email);

        // 驗證登入 Token 是否有效
        // 若驗證成功，回傳對應的 Email
        // 驗證失敗則回傳 false
        bool TryValidateToken(string token, out string email, out string nonce);
    }


}
