using GroupDelivery.Application.Abstractions.Platform;
using GroupDelivery.Domain.Platform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GroupDelivery.Application.Services.Platform
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IUserPermissionRepository _repo;

        public UserPermissionService(IUserPermissionRepository repo)
        {
            _repo = repo;
        }

        public ApiResponse Upsert(UpsertUserPermissionRequest request)
        {
            var entity = new UserPermissionProfile
            {
                UserId = request.UserId,
                RoleName = request.RoleName,
                PermissionsJson = request.PermissionsJson,
                CreatedAt = DateTime.UtcNow
            };
            return ApiResponse.Success("使用者權限已更新", _repo.Upsert(entity));
        }
    }

    public class StoreSubscriptionService : IStoreSubscriptionService
    {
        private readonly IStoreSubscriptionRepository _repo;

        public StoreSubscriptionService(IStoreSubscriptionRepository repo)
        {
            _repo = repo;
        }

        public ApiResponse CreatePlan(CreateSubscriptionRequest request)
        {
            var fee = (decimal)request.Tier;
            if (fee < 0)
            {
                return ApiResponse.Fail("方案費用不可為負");
            }

            var entity = new StoreSubscription
            {
                StoreId = request.StoreId,
                Tier = request.Tier,
                MonthlyFee = fee,
                StartAt = request.StartAt,
                EndAt = request.StartAt.AddMonths(1),
                IsAutoRenew = request.IsAutoRenew
            };

            return ApiResponse.Success("訂閱方案建立成功", _repo.Create(entity));
        }
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public ApiResponse Create(CreateProductRequest request)
        {
            if (request.Price < 0)
            {
                return ApiResponse.Fail("商品價格不可為負");
            }

            var entity = new ProductMenu
            {
                StoreId = request.StoreId,
                ProductName = request.ProductName,
                Price = request.Price,
                IsEnabled = true
            };
            return ApiResponse.Success("商品建立成功", _repo.Create(entity));
        }

        public ApiResponse ListByStore(int storeId)
        {
            return ApiResponse.Success("商品列表", _repo.GetByStoreId(storeId));
        }
    }

    public class GroupOrderCoreService : IGroupOrderCoreService
    {
        private readonly ICampaignRepository _repo;

        public GroupOrderCoreService(ICampaignRepository repo)
        {
            _repo = repo;
        }

        public ApiResponse CreateCampaign(CreateCampaignRequest request)
        {
            var entity = new GroupOrderCampaign
            {
                StoreId = request.StoreId,
                CreatorUserId = request.CreatorUserId,
                Title = request.Title,
                CloseAt = request.CloseAt,
                IsClosed = false
            };
            return ApiResponse.Success("揪團建立成功", _repo.Create(entity));
        }

        public ApiResponse ListOpenCampaigns()
        {
            return ApiResponse.Success("開團中列表", _repo.GetOpenCampaigns());
        }
    }

    public class OrderService : IOrderService
    {
        private readonly IPlatformOrderRepository _repo;

        public OrderService(IPlatformOrderRepository repo)
        {
            _repo = repo;
        }

        public ApiResponse CreateOrder(CreateOrderRequest request)
        {
            if (request.Amount < 0)
            {
                return ApiResponse.Fail("訂單金額不可為負");
            }

            var entity = new PlatformOrder
            {
                CampaignId = request.CampaignId,
                BuyerUserId = request.BuyerUserId,
                Amount = request.Amount,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            return ApiResponse.Success("訂單建立成功", _repo.Create(entity));
        }
    }

    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _repo;

        public DeliveryService(IDeliveryRepository repo)
        {
            _repo = repo;
        }

        public ApiResponse CreateDelivery(CreateDeliveryRequest request)
        {
            var entity = new DeliveryTask
            {
                CampaignId = request.CampaignId,
                Mode = request.Mode,
                DriverName = request.DriverName,
                RouteJson = request.RouteJson,
                Status = "Created"
            };
            return ApiResponse.Success("配送任務建立成功", _repo.Create(entity));
        }
    }

    public class PromotionWeightService : IPromotionWeightService
    {
        private readonly ICampaignRepository _campaignRepo;
        private readonly IPlatformOrderRepository _orderRepo;

        public PromotionWeightService(ICampaignRepository campaignRepo, IPlatformOrderRepository orderRepo)
        {
            _campaignRepo = campaignRepo;
            _orderRepo = orderRepo;
        }

        public ApiResponse RankCampaigns()
        {
            var campaigns = _campaignRepo.GetOpenCampaigns();
            var result = campaigns.Select(c => new
            {
                Campaign = c,
                Weight = _orderRepo.GetByCampaignId(c.Id).Count
            }).OrderByDescending(x => x.Weight).ToList();

            return ApiResponse.Success("推團排序完成", result);
        }
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IPlatformOrderRepository _orderRepo;

        public AnalyticsService(IPlatformOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public ApiResponse GetCampaignAnalytics(int campaignId)
        {
            IList<PlatformOrder> orders = _orderRepo.GetByCampaignId(campaignId);
            var amount = orders.Sum(x => x.Amount);
            var data = new
            {
                CampaignId = campaignId,
                OrderCount = orders.Count,
                TotalAmount = amount,
                AvgAmount = orders.Count == 0 ? 0 : amount / orders.Count
            };
            return ApiResponse.Success("揪團數據分析完成", data);
        }
    }

    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _repo;
        private readonly IStoreSubscriptionRepository _subscriptionRepo;

        public BillingService(IBillingRepository repo, IStoreSubscriptionRepository subscriptionRepo)
        {
            _repo = repo;
            _subscriptionRepo = subscriptionRepo;
        }

        public ApiResponse CreateBilling(CreateBillingRequest request)
        {
            if (request.Amount < 0)
            {
                return ApiResponse.Fail("帳單金額不可為負");
            }

            var entity = new BillingRecord
            {
                StoreId = request.StoreId,
                Amount = request.Amount,
                BillingMonth = request.BillingMonth,
                IsPaid = false,
                IsRenewed = false
            };
            return ApiResponse.Success("月費帳單建立成功", _repo.Create(entity));
        }

        public ApiResponse Renew(int storeId)
        {
            var current = _subscriptionRepo.GetActiveByStoreId(storeId);
            if (current == null)
            {
                return ApiResponse.Fail("找不到可續約方案");
            }

            var next = new StoreSubscription
            {
                StoreId = current.StoreId,
                Tier = current.Tier,
                MonthlyFee = current.MonthlyFee,
                StartAt = current.EndAt,
                EndAt = current.EndAt.AddMonths(1),
                IsAutoRenew = current.IsAutoRenew
            };
            return ApiResponse.Success("續約成功", _subscriptionRepo.Create(next));
        }
    }

    public class AdminService : IAdminService
    {
        private readonly IAdminAuditRepository _repo;

        public AdminService(IAdminAuditRepository repo)
        {
            _repo = repo;
        }

        public ApiResponse WriteAudit(AdminActionRequest request)
        {
            var entity = new AdminAuditLog
            {
                AdminUserId = request.AdminUserId,
                ActionName = request.ActionName,
                Payload = request.Payload,
                CreatedAt = DateTime.UtcNow
            };

            return ApiResponse.Success("後台操作已記錄", _repo.Create(entity));
        }

        public ApiResponse Dashboard()
        {
            return ApiResponse.Success("管理後台摘要", _repo.GetTop(20));
        }
    }
}
