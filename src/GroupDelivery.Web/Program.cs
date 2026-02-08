using AspNet.Security.OAuth.Line;
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using GroupDelivery.Infrastructure.Repositories;
using GroupDelivery.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroupDelivery.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            // MVC + JSON 設定
            
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(o =>
                {
                    // Vue 友善 camelCase
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                    // 避免 EF 循環參考爆炸
                    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });


            // Authentication
            // Cookie = 站內登入
            // LINE = 第三方登入

            builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "GroupDelivery.Auth";
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });
   




            // Email 設定

            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            
            // Database
            
            builder.Services.AddDbContext<GroupDeliveryDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHttpContextAccessor();

            // DI 注入

            // Email 發送服務，負責寄送驗證信、通知信等
            builder.Services.AddScoped<EmailService>();

            // =========================
            // 驗證 / 登入相關
            // =========================

            // 驗證流程主服務（登入、第三方登入、身分驗證）
            builder.Services.AddScoped<IAuthService, AuthService>();

            // 登入用一次性 Token 管理（Email 登入驗證）
            builder.Services.AddScoped<ILoginTokenService, LoginTokenService>();

            // =========================
            // 使用者相關
            // =========================

            // 使用者商業邏輯（個人資料、角色等）
            builder.Services.AddScoped<IUserService, UserService>();

            // 使用者資料存取（tbUser）
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // =========================
            // 團單（GroupOrder）
            // =========================

            // 團單商業邏輯（開團、狀態轉換、過期處理）
            builder.Services.AddScoped<IGroupOrderService, GroupOrderService>();

            // 團單資料存取（tbGroupOrder）
            builder.Services.AddScoped<IGroupOrderRepository, GroupOrderRepository>();

            // =========================
            // 商店（Store）
            // =========================

            // 商店商業邏輯（建立、編輯、休息日管理）
            builder.Services.AddScoped<IStoreService, StoreService>();

            // 商店資料存取（tbStore）
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();

            // =========================
            // 商家 / 群組輔助服務
            // =========================

            // 商家相關邏輯（商家身分、權限）
            builder.Services.AddScoped<IMerchantService, MerchantService>();

            // 群組輔助服務（非團單本體，例如聚合顯示、判斷）
            builder.Services.AddScoped<IGroupService, GroupService>();

            // =========================
            // 商店休息日
            // =========================

            // 商店「臨時休息日」（指定日期）資料存取
            builder.Services.AddScoped<IStoreClosedDateRepository, StoreClosedDateRepository>();

            // 商店「每週固定休息日」資料存取（星期一～日）
            builder.Services.AddScoped<IStoreWeeklyClosedDayRepository, StoreWeeklyClosedDayRepository>();




            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
