using System;

namespace GroupDelivery.Domain.Platform
{
    public enum SubscriptionTier
    {
        Plan299 = 299,
        Plan499 = 499,
        Plan699 = 699,
        Plan999 = 999
    }

    public enum DeliveryMode
    {
        Self = 1,
        Batch = 2,
        Route = 3
    }

    public class UserPermissionProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RoleName { get; set; }
        public string PermissionsJson { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StoreSubscription
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public SubscriptionTier Tier { get; set; }
        public decimal MonthlyFee { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool IsAutoRenew { get; set; }
    }

    public class ProductMenu
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class GroupOrderCampaign
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CreatorUserId { get; set; }
        public string Title { get; set; }
        public DateTime CloseAt { get; set; }
        public bool IsClosed { get; set; }
    }

    public class PlatformOrder
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int BuyerUserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DeliveryTask
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public DeliveryMode Mode { get; set; }
        public string DriverName { get; set; }
        public string RouteJson { get; set; }
        public string Status { get; set; }
    }

    public class BillingRecord
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BillingMonth { get; set; }
        public bool IsPaid { get; set; }
        public bool IsRenewed { get; set; }
    }

    public class AdminAuditLog
    {
        public int Id { get; set; }
        public int AdminUserId { get; set; }
        public string ActionName { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
