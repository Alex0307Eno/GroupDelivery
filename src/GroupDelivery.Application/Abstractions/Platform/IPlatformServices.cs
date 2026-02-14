using GroupDelivery.Domain.Platform;

namespace GroupDelivery.Application.Abstractions.Platform
{
    public interface IUserPermissionService
    {
        ApiResponse Upsert(UpsertUserPermissionRequest request);
    }

    public interface IStoreSubscriptionService
    {
        ApiResponse CreatePlan(CreateSubscriptionRequest request);
    }

    public interface IProductService
    {
        ApiResponse Create(CreateProductRequest request);
        ApiResponse ListByStore(int storeId);
    }

    public interface IGroupOrderCoreService
    {
        ApiResponse CreateCampaign(CreateCampaignRequest request);
        ApiResponse ListOpenCampaigns();
    }

    public interface IOrderService
    {
        ApiResponse CreateOrder(CreateOrderRequest request);
    }

    public interface IDeliveryService
    {
        ApiResponse CreateDelivery(CreateDeliveryRequest request);
    }

    public interface IPromotionWeightService
    {
        ApiResponse RankCampaigns();
    }

    public interface IAnalyticsService
    {
        ApiResponse GetCampaignAnalytics(int campaignId);
    }

    public interface IBillingService
    {
        ApiResponse CreateBilling(CreateBillingRequest request);
        ApiResponse Renew(int storeId);
    }

    public interface IAdminService
    {
        ApiResponse WriteAudit(AdminActionRequest request);
        ApiResponse Dashboard();
    }
}
