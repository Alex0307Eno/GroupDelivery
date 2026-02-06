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

        public string GenerateToken(string email)
        {
            var expire = DateTime.UtcNow.AddMinutes(10);
            var payload = $"{email}|{expire:O}";
            var sign = Sign(payload);

            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{payload}|{sign}")
            );
        }

        public bool TryValidateToken(string token, out string email)
        {
            email = null;

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
            if (parts.Length != 3)
                return false;

            email = parts[0];
            var expireText = parts[1];   
            var sign = parts[2];

            DateTime expire;
            if (!DateTime.TryParse(
                    expireText,
                    null,
                    System.Globalization.DateTimeStyles.RoundtripKind,
                    out expire))
                return false;

            if (expire < DateTime.UtcNow)
                return false;

            var expected = Sign($"{email}|{expireText}");

            return sign == expected;
        }

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
    }
}
