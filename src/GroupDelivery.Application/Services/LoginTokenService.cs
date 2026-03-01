using GroupDelivery.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class LoginTokenService : ILoginTokenService
    {
        private readonly IConfiguration _config;

        public LoginTokenService(IConfiguration config)
        {
            _config = config;
        }
        #region 產生一個有時效性的登入 Token（綁定 Email，10 分鐘內有效）
        public string GenerateToken(string email)
        {
            var expire = DateTime.UtcNow.AddMinutes(10);
            var nonce = Guid.NewGuid().ToString();

            var payload = $"{email}|{expire:O}|{nonce}";
            var sign = Sign(payload);

            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{payload}|{sign}")
            );
        }
        #endregion

        #region 驗證登入 Token 是否有效，成功時取回對應的 Email
        public bool TryValidateToken(string token, out string email, out string nonce)
        {
            email = null;
            nonce = null;

            string raw;

            try
            {
                raw = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            }
            catch
            {
                return false;
            }

            var parts = raw.Split('|');
            if (parts.Length != 4)
                return false;

            var tokenEmail = parts[0];
            var expireText = parts[1];
            var tokenNonce = parts[2];
            var sign = parts[3];

            DateTime expire;
            if (!DateTime.TryParse(
                    expireText,
                    null,
                    System.Globalization.DateTimeStyles.RoundtripKind,
                    out expire))
                return false;

            if (expire < DateTime.UtcNow)
                return false;

            var expected = Sign($"{tokenEmail}|{expireText}|{tokenNonce}");

            if (!SlowEquals(sign, expected))
                return false;

            email = tokenEmail;
            nonce = tokenNonce;

            return true;
        }
        #endregion

        #region 使用系統密鑰對內容進行簽章，用於防止 Token 被竄改
        private string Sign(string text)
        {
            var secret = _config["App:LoginSecret"];
            if (string.IsNullOrWhiteSpace(secret))
                secret = _config["AppLoginSecret"];

            if (string.IsNullOrWhiteSpace(secret))
                throw new Exception("App LoginSecret 未設定");

            var key = Encoding.UTF8.GetBytes(secret);
            using (var hmac = new HMACSHA256(key))
            {
                return Convert.ToBase64String(
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(text))
                );
            }
        }
        #endregion

        private bool SlowEquals(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);

            if (aBytes.Length != bBytes.Length)
                return false;

            int diff = 0;

            for (int i = 0; i < aBytes.Length; i++)
            {
                diff |= aBytes[i] ^ bBytes[i];
            }

            return diff == 0;
        }
    }
}
