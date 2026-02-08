using GroupDelivery.Domain;
using Microsoft.EntityFrameworkCore;

namespace GroupDelivery.Infrastructure.Data
{
    public class GroupDeliveryDbContext : DbContext
    {
        public GroupDeliveryDbContext(DbContextOptions<GroupDeliveryDbContext> options)
            : base(options)
        {
        }
        

       

        // =========================
        // 使用者與商家相關
        // =========================

        // 系統使用者（一般使用者 / 商家 / 管理者）
        public DbSet<User> Users { get; set; }

        // 商店主體資料
        public DbSet<Store> Stores { get; set; }

        //團購資料
        public DbSet<Group> Groups { get; set; }

        // =========================
        // 揪團 / 團購相關
        // =========================

        // 揪團主檔（團狀態、金額、截止時間等）
        public DbSet<GroupOrder> GroupOrders { get; set; }

        

        // =========================
        // 商店營業狀態設定
        // =========================

        // 指定日期的休息日（例：國定假日、臨時公休）
        public DbSet<StoreClosedDate> StoreClosedDates { get; set; }

        // 每週固定休息日（例：每週一、每週日）
        public DbSet<StoreWeeklyClosedDay> StoreWeeklyClosedDays { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
