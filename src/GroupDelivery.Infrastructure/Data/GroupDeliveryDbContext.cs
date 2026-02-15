using GroupDelivery.Domain;
using GroupDelivery.Domain.Platform;
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

        public DbSet<UserPermissionProfile> UserPermissionProfiles { get; set; }
        public DbSet<StoreSubscription> StoreSubscriptions { get; set; }
        public DbSet<ProductMenu> ProductMenus { get; set; }
        public DbSet<GroupOrderCampaign> GroupOrderCampaigns { get; set; }
        public DbSet<PlatformOrder> PlatformOrders { get; set; }
        public DbSet<DeliveryTask> DeliveryTasks { get; set; }
        public DbSet<BillingRecord> BillingRecords { get; set; }
        public DbSet<AdminAuditLog> AdminAuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StoreMenuItem>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<GroupOrderItem>()
                .Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<GroupOrderItem>()
                .Property(x => x.SubTotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<StoreSubscription>()
                .Property(x => x.MonthlyFee)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductMenu>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PlatformOrder>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<BillingRecord>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

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
        }
    }
}
