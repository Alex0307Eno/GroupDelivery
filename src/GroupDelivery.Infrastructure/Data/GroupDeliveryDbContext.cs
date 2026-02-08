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
        public DbSet<EmailLoginToken> EmailLoginTokens { get; set; }

        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupOrder> GroupOrders { get; set; }
        public DbSet<GroupOrderItem> GroupOrderItems { get; set; }

        public DbSet<StoreClosedDate> StoreClosedDates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
