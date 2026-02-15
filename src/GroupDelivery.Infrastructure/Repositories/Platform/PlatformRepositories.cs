using GroupDelivery.Application.Abstractions.Platform;
using GroupDelivery.Domain.Platform;
using GroupDelivery.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace GroupDelivery.Infrastructure.Repositories.Platform
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public UserPermissionRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public UserPermissionProfile Upsert(UserPermissionProfile entity)
        {
            var old = _db.UserPermissionProfiles.FirstOrDefault(x => x.UserId == entity.UserId);
            if (old == null)
            {
                _db.UserPermissionProfiles.Add(entity);
            }
            else
            {
                old.RoleName = entity.RoleName;
                old.PermissionsJson = entity.PermissionsJson;
                entity = old;
            }

            _db.SaveChanges();
            return entity;
        }
    }

    public class StoreSubscriptionRepository : IStoreSubscriptionRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreSubscriptionRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public StoreSubscription Create(StoreSubscription entity)
        {
            _db.StoreSubscriptions.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public StoreSubscription GetActiveByStoreId(int storeId)
        {
            return _db.StoreSubscriptions.Where(x => x.StoreId == storeId)
                .OrderByDescending(x => x.EndAt)
                .FirstOrDefault();
        }
    }

    public class ProductRepository : IProductRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public ProductRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public ProductMenu Create(ProductMenu entity)
        {
            _db.ProductMenus.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public IList<ProductMenu> GetByStoreId(int storeId)
        {
            return _db.ProductMenus.Where(x => x.StoreId == storeId).ToList();
        }
    }

    public class CampaignRepository : ICampaignRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public CampaignRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public GroupOrderCampaign Create(GroupOrderCampaign entity)
        {
            _db.GroupOrderCampaigns.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public IList<GroupOrderCampaign> GetOpenCampaigns()
        {
            return _db.GroupOrderCampaigns.Where(x => !x.IsClosed).ToList();
        }
    }

    public class PlatformOrderRepository : IPlatformOrderRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public PlatformOrderRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public PlatformOrder Create(PlatformOrder entity)
        {
            _db.PlatformOrders.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public IList<PlatformOrder> GetByCampaignId(int campaignId)
        {
            return _db.PlatformOrders.Where(x => x.CampaignId == campaignId).ToList();
        }
    }

    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public DeliveryRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public DeliveryTask Create(DeliveryTask entity)
        {
            _db.DeliveryTasks.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public IList<DeliveryTask> GetByCampaignId(int campaignId)
        {
            return _db.DeliveryTasks.Where(x => x.CampaignId == campaignId).ToList();
        }
    }

    public class BillingRepository : IBillingRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public BillingRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public BillingRecord Create(BillingRecord entity)
        {
            _db.BillingRecords.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public IList<BillingRecord> GetByStoreId(int storeId)
        {
            return _db.BillingRecords.Where(x => x.StoreId == storeId).ToList();
        }
    }

    public class AdminAuditRepository : IAdminAuditRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public AdminAuditRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public AdminAuditLog Create(AdminAuditLog entity)
        {
            _db.AdminAuditLogs.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public IList<AdminAuditLog> GetTop(int count)
        {
            return _db.AdminAuditLogs.OrderByDescending(x => x.CreatedAt).Take(count).ToList();
        }
    }
}
