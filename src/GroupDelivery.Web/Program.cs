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

            builder.Services.AddScoped<EmailService>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILoginTokenService, LoginTokenService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IGroupOrderService, GroupOrderService>();
            builder.Services.AddScoped<IGroupOrderRepository, GroupOrderRepository>();

            builder.Services.AddScoped<IStoreService, StoreService>();
            builder.Services.AddScoped<IStoreRepository, StoreRepository>();
            builder.Services.AddScoped<IMerchantService, MerchantService>();


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
