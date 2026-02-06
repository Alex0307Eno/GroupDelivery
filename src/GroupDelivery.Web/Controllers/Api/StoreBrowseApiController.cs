using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    public class StoreBrowseApiController : ControllerBase
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreBrowseApiController(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        [HttpGet("/api/stores/nearby")]
        public async Task<IActionResult> NearbyStores()
        {
            var activeStoreIds = await _db.GroupOrders
                .Where(x => x.Status == GroupOrderStatus.Open)
                .Select(x => x.StoreId)
                .Distinct()
                .ToListAsync();

            var stores = await _db.Stores
                .Select(x => new
                {
                    storeId = x.StoreId,
                    storeName = x.StoreName,
                    distance = 0,
                    hasActiveGroup = activeStoreIds.Contains(x.StoreId)
                })
                .ToListAsync();

            return Ok(stores);
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
