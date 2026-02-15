using System;

namespace GroupDelivery.Domain.Platform
{
    public class UpsertUserPermissionRequest
    {
        public int UserId { get; set; }
        public string RoleName { get; set; }
        public string PermissionsJson { get; set; }
    }

    public class CreateSubscriptionRequest
    {
        public int StoreId { get; set; }
        public SubscriptionTier Tier { get; set; }
        public DateTime StartAt { get; set; }
        public bool IsAutoRenew { get; set; }
    }

    public class CreateProductRequest
    {
        public int StoreId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateCampaignRequest
    {
        public int StoreId { get; set; }
        public int CreatorUserId { get; set; }
        public string Title { get; set; }
        public DateTime CloseAt { get; set; }
    }

    public class CreateOrderRequest
    {
        public int CampaignId { get; set; }
        public int BuyerUserId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateDeliveryRequest
    {
        public int CampaignId { get; set; }
        public DeliveryMode Mode { get; set; }
        public string DriverName { get; set; }
        public string RouteJson { get; set; }
    }

    public class CreateBillingRequest
    {
        public int StoreId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillingMonth { get; set; }
    }

    public class AdminActionRequest
    {
        public int AdminUserId { get; set; }
        public string ActionName { get; set; }
        public string Payload { get; set; }
    }
}
