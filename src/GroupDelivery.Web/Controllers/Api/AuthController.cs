using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using AspNet.Security.OAuth.Line;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;


namespace GroupDelivery.Web.Controllers.Api
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult LineLogin()
        {
            var redirectUri = Uri.EscapeDataString(
                "https://b3dfaabfa881.ngrok-free.app/signin-line"
            );

            var url =
                "https://access.line.me/oauth2/v2.1/authorize" +
                "?response_type=code" +
                "&client_id=" + _config["Line:ChannelId"] +
                "&redirect_uri=" + redirectUri +
                "&scope=profile%20openid" +
                "&state=dev";

            return Redirect(url);
        }

        [HttpGet("/signin-line")]
        public async Task<IActionResult> LineCallback(
    string code,
    [FromServices] GroupDeliveryDbContext db)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("No code");
            }

            var client = new HttpClient();

            //  用 code 換 token
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", "https://b3dfaabfa881.ngrok-free.app/signin-line" },
                { "client_id", _config["Line:ChannelId"] },
                { "client_secret", _config["Line:ChannelSecret"] }
            });

            var tokenRes = await client.PostAsync(
                "https://api.line.me/oauth2/v2.1/token",
                content
            );

            if (!tokenRes.IsSuccessStatusCode)
            {
                var error = await tokenRes.Content.ReadAsStringAsync();
                return BadRequest("Token exchange failed: " + error);
            }

            var tokenJson = await tokenRes.Content.ReadAsStringAsync();
            var tokenDoc = JsonDocument.Parse(tokenJson);

            var accessToken = tokenDoc.RootElement
                .GetProperty("access_token")
                .GetString();

            //  用 access_token 拿 LINE 使用者資料
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var profileJson = await client.GetStringAsync(
                "https://api.line.me/v2/profile");

            var profileDoc = JsonDocument.Parse(profileJson);
            var lineUserId = profileDoc.RootElement.GetProperty("userId").GetString();
            var displayName = profileDoc.RootElement.GetProperty("displayName").GetString();
            var pictureUrl = profileDoc.RootElement
                .GetProperty("pictureUrl")
                .GetString();


            

            //  查 DB，有沒有這個 LINE 使用者
            var user = await db.Users
                .FirstOrDefaultAsync(x => x.LineUserId == lineUserId);

            if (user == null)
            {
                // 新使用者
                user = new User
                {
                    LineUserId = lineUserId,
                    DisplayName = displayName,
                    PictureUrl = pictureUrl,
                    Role = UserRole.None,
                    CreatedAt = DateTime.Now
                };

                db.Users.Add(user);
            }
            else
            {
                // 舊使用者，登入時順便同步
                user.DisplayName = displayName;
                user.PictureUrl = pictureUrl;
            }

            
            await db.SaveChangesAsync();

            //  發你自己系統的登入 Cookie
            var claims = new List<System.Security.Claims.Claim>
    {
        new System.Security.Claims.Claim(
            System.Security.Claims.ClaimTypes.NameIdentifier,
            user.UserId.ToString()),
        new System.Security.Claims.Claim(
            System.Security.Claims.ClaimTypes.Name,
            user.DisplayName)
    };

            var identity = new System.Security.Claims.ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new System.Security.Claims.ClaimsPrincipal(identity));

            // 回首頁
            if (user.Role == UserRole.None)
            {
                return Redirect("/Onboarding/ChooseRole");
            }

            return Redirect("/");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/");
        }

    }
}
