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
        public DbSet<LoginTokenUsage> LoginTokenUsages { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<GroupOrder> GroupOrders { get; set; }
        public DbSet<StoreMenuCategory> StoreMenuCategories { get; set; }

        public DbSet<StoreMenuItem> StoreMenuItems { get; set; }
        public DbSet<GroupOrderItem> GroupOrderItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<StoreMenuItemOptionGroup> StoreMenuItemOptionGroups { get; set; }
        public DbSet<StoreMenuItemOption> StoreMenuItemOptions { get; set; }
        public DbSet<OrderItemOption> OrderItemOptions { get; set; }
        public DbSet<DeliveryRule> DeliveryRules { get; set; }
        public DbSet<MerchantSubscription> MerchantSubscriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GroupOrder>()
        .ToTable("tbGroupOrder");
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
           
        
            modelBuilder.Entity<Store>()
                .HasIndex(x => x.StorePublicId)
                .IsUnique();

            modelBuilder.Entity<GroupOrder>()
                .HasIndex(x => x.GroupOrderPublicId)
                .IsUnique();

            modelBuilder.Entity<StoreMenuItem>()
                .HasIndex(x => x.StoreMenuItemPublicId)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasIndex(x => x.OrderPublicId)
                .IsUnique();
        
        }
    }
}
