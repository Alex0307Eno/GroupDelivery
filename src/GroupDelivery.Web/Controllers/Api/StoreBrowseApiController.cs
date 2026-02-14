using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using GroupDelivery.Application.Abstractions;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    public class StoreBrowseApiController : ControllerBase
    {
        private readonly GroupDeliveryDbContext _db;
        private readonly IStoreService _storeService;

        public StoreBrowseApiController(GroupDeliveryDbContext db, IStoreService storeService)
        {
            _db = db;
            _storeService = storeService;
        }

        [HttpGet("/api/stores/nearby")]
        public async Task<IActionResult> NearbyStores()
        {
            var result = await _storeService.GetNearbyStoresAsync();
            return Ok(result);
        }

        [HttpGet("/api/store/groups")]
        public async Task<IActionResult> StoreGroups()
        {
            var groups = await _db.GroupOrders
                .Include(x => x.Store)
                .Where(x => x.Status == GroupOrderStatus.Open)
                .Select(x => new
                {
                    groupOrderId = x.GroupOrderId,
                    remark = x.Remark,
                    deadline = x.Deadline,
                    currentPeople = x.CurrentAmount,
                    targetPeople = x.TargetAmount,
                    storeName = x.Store.StoreName
                })
                .ToListAsync();

            return Ok(groups);
        }
    }
}
