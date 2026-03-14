using GroupDelivery.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

            // 全域預設關閉 Cascade Delete，避免多重串聯路徑
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            // 金額欄位精度
            modelBuilder.Entity<StoreMenuItem>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<GroupOrderItem>()
                .Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<GroupOrderItem>()
                .Property(x => x.SubTotal)
                .HasColumnType("decimal(18,2)");

            // 主鍵
            modelBuilder.Entity<StoreMenuItemOption>()
                .HasKey(x => x.StoreMenuItemOptionId);

            modelBuilder.Entity<StoreMenuItemOptionGroup>()
                .HasKey(x => x.StoreMenuItemOptionGroupId);

            // 唯一索引
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

            // Store -> GroupOrder
            modelBuilder.Entity<GroupOrder>()
                .HasOne(x => x.Store)
                .WithMany(x => x.GroupOrders)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.NoAction);

            // GroupOrder -> OwnerUser
            modelBuilder.Entity<GroupOrder>()
                .HasOne(x => x.OwnerUser)
                .WithMany()
                .HasForeignKey(x => x.OwnerUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Store -> Category
            modelBuilder.Entity<StoreMenuCategory>()
                .HasOne(x => x.Store)
                .WithMany(x => x.StoreMenuCategories)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.NoAction);

            // Store -> MenuItem
            modelBuilder.Entity<StoreMenuItem>()
                .HasOne(x => x.Store)
                .WithMany(x => x.StoreMenuItems)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.NoAction);

            // Category -> MenuItem
            modelBuilder.Entity<StoreMenuItem>()
                .HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            // MenuItem -> GroupOrderItem
            modelBuilder.Entity<GroupOrderItem>()
                .HasOne(x => x.StoreMenuItem)
                .WithMany(x => x.GroupOrderItems)
                .HasForeignKey(x => x.StoreMenuItemId)
                .OnDelete(DeleteBehavior.NoAction);

            // GroupOrder -> GroupOrderItem
            modelBuilder.Entity<GroupOrderItem>()
                .HasOne(x => x.GroupOrder)
                .WithMany(x => x.GroupOrderItems)
                .HasForeignKey(x => x.GroupOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            // MenuItem -> OptionGroup
            modelBuilder.Entity<StoreMenuItemOptionGroup>()
                .HasOne(x => x.StoreMenuItem)
                .WithMany()
                .HasForeignKey(x => x.StoreMenuItemId)
                .OnDelete(DeleteBehavior.NoAction);

            // OptionGroup -> Option
            modelBuilder.Entity<StoreMenuItemOption>()
                .HasOne(x => x.OptionGroup)
                .WithMany()
                .HasForeignKey(x => x.OptionGroupId)
                .OnDelete(DeleteBehavior.NoAction);

            // DeliveryRule -> Store
            modelBuilder.Entity<DeliveryRule>()
                .HasOne(x => x.Store)
                .WithMany(x => x.DeliveryRules)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}