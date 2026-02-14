using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
            var activeGroups = await _db.GroupOrders
                .Where(g => g.Status == GroupOrderStatus.Open)
                .GroupBy(g => g.StoreId)
                .Select(g => new
                {
                    StoreId = g.Key,
                    Deadline = g.Min(x => x.Deadline),
                    CreatedAt = g.Max(x => x.CreatedAt)
                })
                .ToListAsync();

            var activeGroupMap = activeGroups.ToDictionary(x => x.StoreId, x => x);

            var stores = await _db.Stores
                .Select(s => new
                {
                    storeId = s.StoreId,
                    storeName = s.StoreName,
                    address = s.Address,
                    distance = 0,
                    hasActiveGroup = activeGroupMap.ContainsKey(s.StoreId),
                    activeGroupDeadline = activeGroupMap.ContainsKey(s.StoreId)
                        ? activeGroupMap[s.StoreId].Deadline
                        : (DateTime?)null,
                    activeGroupCreatedAt = activeGroupMap.ContainsKey(s.StoreId)
                        ? activeGroupMap[s.StoreId].CreatedAt
                        : (DateTime?)null,
                    coverImageUrl = s.CoverImageUrl
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
