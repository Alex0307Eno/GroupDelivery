using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }
        #region 取得指定店家資料
        public async Task<Store> GetByIdAndOwnerAsync(int storeId, int ownerUserId)
        {
            return await _db.Stores
                .FirstOrDefaultAsync(x =>
                    x.StoreId == storeId &&
                    x.OwnerUserId == ownerUserId);
        }
        #endregion

        #region 取得指定使用者的所有店家資料
        public async Task<List<Store>> GetByOwnerAsync(int ownerUserId)
        {
            return await _db.Stores
                .Where(x => x.OwnerUserId == ownerUserId)
                .ToListAsync();
        }
        #endregion
        public async Task<Store> GetByGuIdAsync(Guid publicId)
        {
            return await _db.Stores
                .FirstOrDefaultAsync(x => x.StorePublicId == publicId);
        }
        #region 取得指定使用者的第一間店家資料
        public async Task<Store> GetFirstByOwnerAsync(int ownerUserId)
        {
            return await _db.Stores
                .Where(x => x.OwnerUserId == ownerUserId)
                .OrderBy(x => x.StoreId)
                .FirstOrDefaultAsync();
        }
        public async Task<Store> GetByPublicIdAndOwnerAsync(Guid publicId, int ownerUserId)
        {
            return await _db.Stores
                .FirstOrDefaultAsync(x =>
                    x.StorePublicId == publicId &&
                    x.OwnerUserId == ownerUserId);
        }
        #endregion

        #region 商家CRUD
        public async Task<Guid> CreateAsync(Store store)
        {
            store.StorePublicId = Guid.NewGuid();

            await _db.Stores.AddAsync(store);
            await _db.SaveChangesAsync();

            return store.StorePublicId;
        }

        public async Task UpdateAsync(Store store)
        {
            _db.Stores.Update(store);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Store store)
        {
            _db.Stores.Remove(store);
            await _db.SaveChangesAsync();
        }
        #endregion
        public async Task<Store> GetByIdAsync(int storeId)
        {
            return await _db.Stores
                .FirstOrDefaultAsync(s => s.StoreId == storeId);
        }
        public async Task<List<Store>> GetAllAsync()
        {
            return await _db.Stores.ToListAsync();
        }
        public async Task<Store> GetByPublicIdAsync(Guid storePublicId)
        {
            return await _db.Stores
                .FirstOrDefaultAsync(x => x.StorePublicId == storePublicId);
        }
        public async Task<List<Store>> GetByOwnerUserIdAsync(int userId)
        {
            return await _db.Stores
                .Where(x => x.OwnerUserId == userId)
                .ToListAsync();
        }
        public async Task<List<StoreNearbyDto>> GetNearbyStoresAsync(double? lat, double? lng, string city)
        {
            var now = DateTime.Now;

            var query = _db.Stores
                .AsNoTracking()
                .Where(s => s.GroupOrders.Any(g => g.Status == GroupOrderStatus.Open));

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(x => x.City == city);
            }

            var stores = await query
                .Select(x => new
                {
                    x.StoreId,
                    x.StoreName,
                    x.Address,
                    x.City,
                    x.CoverImageUrl,
                    BusinessHours = x.OpenTime.ToString() + " - " + x.CloseTime.ToString(),
                    IsOpenNow = !x.IsPausedToday &&
                                            now.TimeOfDay >= x.OpenTime &&
                                            now.TimeOfDay <= x.CloseTime,
                    x.Latitude,
                    x.Longitude,
                    ActiveGroup = x.GroupOrders
                        .Where(g => (int)g.Status == (int)GroupOrderStatus.Open && g.Deadline > now)
                        .OrderBy(g => g.Deadline)
                        .Select(g => new
                        {
                            g.Deadline
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            var result = stores.Select(x => new StoreNearbyDto
            {
                StoreId = x.StoreId,
                StoreName = x.StoreName,
                Address = x.Address,
                City = x.City,
                CoverImageUrl = x.CoverImageUrl,
                BusinessHours = x.BusinessHours,
                IsOpenNow = x.IsOpenNow,
                HasActiveGroup = x.ActiveGroup != null,
                ActiveGroupDeadline = x.ActiveGroup == null
                    ? (DateTime?)null
                    : x.ActiveGroup.Deadline,
                Distance = CalculateDistanceInMeters(lat, lng, x.Latitude, x.Longitude)
            })
            .OrderBy(x => x.Distance.HasValue ? x.Distance.Value : double.MaxValue)
            .ToList();

            return result;
        }
        // 計算兩點之間距離，單位為公尺
        private double? CalculateDistanceInMeters(double? userLat, double? userLng, double? storeLat, double? storeLng)
        {
            if (!userLat.HasValue || !userLng.HasValue || !storeLat.HasValue || !storeLng.HasValue)
            {
                return null;
            }

            const double earthRadius = 6371000;

            var dLat = ToRadians(storeLat.Value - userLat.Value);
            var dLng = ToRadians(storeLng.Value - userLng.Value);

            var lat1 = ToRadians(userLat.Value);
            var lat2 = ToRadians(storeLat.Value);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadius * c;
        }

        private double ToRadians(double degree)
        {
            return degree * Math.PI / 180.0;
        }
    }
}

