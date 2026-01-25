using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GroupDelivery.Infrastructure.Data;
using GroupDelivery.Application.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroupDelivery.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 註冊 MVC + API（唯一一次）
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(o =>
                {
                    // 小寫 JSON 字串（Vue 才吃得到）
                    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                    // 避免循環參考
                    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // 註冊資料庫
            builder.Services.AddDbContext<GroupDeliveryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // DI 注入
            builder.Services.AddScoped<GroupOrderRepository>();
            builder.Services.AddScoped<GroupOrderService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
