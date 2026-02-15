using AspNet.Security.OAuth.Line;
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;


namespace GroupDelivery.Web.Controllers.Api
{
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;


        public AuthController(
            EmailService emailService,
            IConfiguration config,
            IAuthService authService)
        {
            _emailService = emailService;
            _config = config;
            _authService = authService;
        }

        [HttpPost("SendLoginLink")]
        public async Task<IActionResult> SendLoginLink([FromBody] SendLoginRequest req)
        {
            await _authService.SendLoginLinkAsync(req.Email);
            return Ok();
        }
        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            await _authService.SignInByTokenAsync(token, HttpContext);
            
            return Redirect("/Account/AfterLogin");
        }
        [HttpGet("LineLogin")]
        public IActionResult LineLogin()
        {


            var redirectUri = Uri.EscapeDataString(
                _config["Line:BaseUrl"] + "/signin-line"
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
        public async Task<IActionResult> LineCallback(string code,[FromServices] GroupDeliveryDbContext db)
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
                { "redirect_uri",  _config["Line:BaseUrl"] + "/signin-line" },
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

            var currentStore = await db.Stores
                .Where(x => x.OwnerUserId == user.UserId)
                .OrderBy(x => x.StoreId)
                .FirstOrDefaultAsync();

            //  發你自己系統的登入 Cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("LineUserId", user.LineUserId),
                new Claim("StoreId", (currentStore?.StoreId ?? 0).ToString())
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

        

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/");
        }

    }
}
