using GroupDelivery.Application.Abstractions.Platform;
using GroupDelivery.Domain.Platform;
using Microsoft.AspNetCore.Mvc;

namespace GroupDelivery.Web.Controllers.Api.Platform
{
    [ApiController]
    [Route("api/platform/user-permission")]
    public class UserPermissionController : ControllerBase
    {
        private readonly IUserPermissionService _service;

        public UserPermissionController(IUserPermissionService service)
        {
            _service = service;
        }

        [HttpPost("upsert")]
        public IActionResult Upsert([FromBody] UpsertUserPermissionRequest request)
        {
            return Ok(_service.Upsert(request));
        }
    }

    [ApiController]
    [Route("api/platform/store-subscription")]
    public class StoreSubscriptionController : ControllerBase
    {
        private readonly IStoreSubscriptionService _service;

        public StoreSubscriptionController(IStoreSubscriptionService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateSubscriptionRequest request)
        {
            return Ok(_service.CreatePlan(request));
        }
    }

    [ApiController]
    [Route("api/platform/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateProductRequest request)
        {
            return Ok(_service.Create(request));
        }

        [HttpGet("store/{storeId}")]
        public IActionResult ListByStore(int storeId)
        {
            return Ok(_service.ListByStore(storeId));
        }
    }

    [ApiController]
    [Route("api/platform/group-order")]
    public class GroupOrderCoreController : ControllerBase
    {
        private readonly IGroupOrderCoreService _service;

        public GroupOrderCoreController(IGroupOrderCoreService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateCampaignRequest request)
        {
            return Ok(_service.CreateCampaign(request));
        }

        [HttpGet("open")]
        public IActionResult OpenCampaigns()
        {
            return Ok(_service.ListOpenCampaigns());
        }
    }

    [ApiController]
    [Route("api/platform/order")]
    public class PlatformOrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public PlatformOrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateOrderRequest request)
        {
            return Ok(_service.CreateOrder(request));
        }
    }

    [ApiController]
    [Route("api/platform/delivery")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _service;

        public DeliveryController(IDeliveryService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateDeliveryRequest request)
        {
            return Ok(_service.CreateDelivery(request));
        }
    }

    [ApiController]
    [Route("api/platform/promotion")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionWeightService _service;

        public PromotionController(IPromotionWeightService service)
        {
            _service = service;
        }

        [HttpGet("rank")]
        public IActionResult Rank()
        {
            return Ok(_service.RankCampaigns());
        }
    }

    [ApiController]
    [Route("api/platform/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _service;

        public AnalyticsController(IAnalyticsService service)
        {
            _service = service;
        }

        [HttpGet("campaign/{campaignId}")]
        public IActionResult Campaign(int campaignId)
        {
            return Ok(_service.GetCampaignAnalytics(campaignId));
        }
    }

    [ApiController]
    [Route("api/platform/billing")]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _service;

        public BillingController(IBillingService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateBillingRequest request)
        {
            return Ok(_service.CreateBilling(request));
        }

        [HttpPost("renew/{storeId}")]
        public IActionResult Renew(int storeId)
        {
            return Ok(_service.Renew(storeId));
        }
    }

    [ApiController]
    [Route("api/platform/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [HttpPost("audit")]
        public IActionResult Audit([FromBody] AdminActionRequest request)
        {
            return Ok(_service.WriteAudit(request));
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok(_service.Dashboard());
        }
    }
}
