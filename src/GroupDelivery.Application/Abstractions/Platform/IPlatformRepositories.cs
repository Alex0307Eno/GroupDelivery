using GroupDelivery.Domain.Platform;
using System.Collections.Generic;

namespace GroupDelivery.Application.Abstractions.Platform
{
    public interface IUserPermissionRepository
    {
        UserPermissionProfile Upsert(UserPermissionProfile entity);
    }

    public interface IStoreSubscriptionRepository
    {
        StoreSubscription Create(StoreSubscription entity);
        StoreSubscription GetActiveByStoreId(int storeId);
    }

    public interface IProductRepository
    {
        ProductMenu Create(ProductMenu entity);
        IList<ProductMenu> GetByStoreId(int storeId);
    }

    public interface ICampaignRepository
    {
        GroupOrderCampaign Create(GroupOrderCampaign entity);
        IList<GroupOrderCampaign> GetOpenCampaigns();
    }

    public interface IPlatformOrderRepository
    {
        PlatformOrder Create(PlatformOrder entity);
        IList<PlatformOrder> GetByCampaignId(int campaignId);
    }

    public interface IDeliveryRepository
    {
        DeliveryTask Create(DeliveryTask entity);
        IList<DeliveryTask> GetByCampaignId(int campaignId);
    }

    public interface IBillingRepository
    {
        BillingRecord Create(BillingRecord entity);
        IList<BillingRecord> GetByStoreId(int storeId);
    }

    public interface IAdminAuditRepository
    {
        AdminAuditLog Create(AdminAuditLog entity);
        IList<AdminAuditLog> GetTop(int count);
    }
}
