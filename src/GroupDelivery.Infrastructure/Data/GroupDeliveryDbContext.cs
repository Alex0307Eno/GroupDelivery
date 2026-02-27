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

        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupOrder> GroupOrders { get; set; }
        public DbSet<StoreMenuCategory> StoreMenuCategories { get; set; }

        public DbSet<StoreMenuItem> StoreMenuItems { get; set; }
        public DbSet<GroupOrderItem> GroupOrderItems { get; set; }
        public DbSet<StoreMenu> StoreMenus { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<StoreMenuItemOptionGroup> StoreMenuItemOptionGroups { get; set; }
        public DbSet<StoreMenuItemOption> StoreMenuItemOptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MenuItem 價格精度
            modelBuilder.Entity<StoreMenuItem>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<GroupOrderItem>()
                .Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<GroupOrderItem>()
                .Property(x => x.SubTotal)
                .HasColumnType("decimal(18,2)");

            // 關聯設定
            modelBuilder.Entity<GroupOrderItem>()
                .HasOne(x => x.StoreMenuItem)
                .WithMany(x => x.GroupOrderItems)
                .HasForeignKey(x => x.StoreMenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupOrderItem>()
                .HasOne(x => x.GroupOrder)
                .WithMany(x => x.GroupOrderItems)
                .HasForeignKey(x => x.GroupOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupOrder>()
                .HasOne(g => g.OwnerUser)
                .WithMany()
                .HasForeignKey(g => g.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StoreMenuItem>()
                .HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StoreMenuItemOption>()
        .HasKey(x => x.StoreMenuItemOptionId);

            modelBuilder.Entity<StoreMenuItemOptionGroup>()
                .HasKey(x => x.StoreMenuItemOptionGroupId);

        }
    }
}
