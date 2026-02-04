using GroupDelivery.Application.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;


namespace GroupDelivery.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILoginTokenService _tokenService;
        private readonly IUserRepository _userRepo;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthService(
            ILoginTokenService tokenService,
            IUserRepository userRepo,
            EmailService emailService,
            IConfiguration config,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService;
            _userRepo = userRepo;
            _emailService = emailService;
            _config = config;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendLoginLinkAsync(string email)
        {
            var token = _tokenService.GenerateToken(email);

            var encodedToken = Uri.EscapeDataString(token);

            var link = $"{_config["App:BaseUrl"]}/Auth/VerifyEmail?token={encodedToken}";

            await _emailService.SendLoginMail(email, link);
        }


        public async Task SignInByTokenAsync(string token, HttpContext httpContext)
        {
            if (!_tokenService.TryValidateToken(token, out var email))
                throw new Exception("Token 無效");

            var user = await _userRepo.GetOrCreateByEmail(email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
        public bool IsProfileCompleted(ClaimsPrincipal user)
        {
            var userIdText = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdText))
                return false;

            if (!int.TryParse(userIdText, out var userId))
                return false;

            var dbUser = _userRepository.GetByIdAsync(userId).Result;
            if (dbUser == null)
                return false;

            return !string.IsNullOrWhiteSpace(dbUser.DisplayName)
                && !string.IsNullOrWhiteSpace(dbUser.Phone);
        }

        public async Task RefreshSignInAsync(int userId)
        {
            var user = _userRepo.GetById(userId);
            if (user == null)
                throw new Exception("User not found");

            var claims = new List<Claim>
    {
        // 🔑 核心：系統 UserId 一定要放在 NameIdentifier
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),

        new Claim(ClaimTypes.Name, user.DisplayName ?? "使用者"),

        // 角色用標準 ClaimTypes.Role
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var context = _httpContextAccessor.HttpContext;
            if (context == null)
                throw new Exception("HttpContext is null");

            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
        }
    }

}
