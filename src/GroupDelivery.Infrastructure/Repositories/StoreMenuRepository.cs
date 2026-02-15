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
    public class StoreMenuRepository : IStoreMenuRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreMenuRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public Task AddAsync(StoreMenuItem item)
        {
            _db.StoreMenuItems.Add(item);
            return Task.CompletedTask;
        }

        public async Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuItems
                .Where(x => x.StoreId == storeId)
                .OrderBy(x => x.CategoryId)
                .ThenBy(x => x.StoreMenuItemId)
                .ToListAsync();
        }

        public async Task<StoreMenuItem> GetByIdAsync(int id)
        {
            return await _db.StoreMenuItems
                .FirstOrDefaultAsync(x => x.StoreMenuItemId == id);
        }

        public async Task UpdateAsync(StoreMenuItem item)
        {
            _db.StoreMenuItems.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<List<StoreMenuCategory>> GetCategoriesByIdsAsync(List<int> categoryIds)
        {
            return await _db.StoreMenuCategories
                .Where(x => categoryIds.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task ReorderCategoriesAsync(List<CategoryReorderRequest> request)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            var ids = request.Select(x => x.CategoryId).Distinct().ToList();
            var categories = await _db.StoreMenuCategories.Where(x => ids.Contains(x.Id)).ToListAsync();

            foreach (var item in request)
            {
                var category = categories.First(x => x.Id == item.CategoryId);
                category.SortOrder = item.SortOrder;
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }

        public async Task BatchUpdateCategoryActiveAsync(List<CategoryActiveRequest> request)
        {
            var ids = request.Select(x => x.CategoryId).Distinct().ToList();
            var categories = await _db.StoreMenuCategories.Where(x => ids.Contains(x.Id)).ToListAsync();

            foreach (var item in request)
            {
                var category = categories.First(x => x.Id == item.CategoryId);
                category.IsActive = item.IsActive;
            }

            await _db.SaveChangesAsync();
        }

        public async Task TransferCategoryAsync(int sourceCategoryId, int targetCategoryId)
        {
            var items = await _db.StoreMenuItems.Where(x => x.CategoryId == sourceCategoryId).ToListAsync();
            foreach (var item in items)
            {
                item.CategoryId = targetCategoryId;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<List<StoreMenuItem>> GetAvailableItemsAsync(int storeId, TimeSpan time)
        {
            return await _db.StoreMenuItems
                .Where(x => x.StoreId == storeId && x.IsActive)
                .Where(x =>
                    (!x.AvailableStartTime.HasValue || !x.AvailableEndTime.HasValue)
                    || (x.AvailableStartTime <= x.AvailableEndTime
                        && x.AvailableStartTime <= time
                        && time <= x.AvailableEndTime)
                    || (x.AvailableStartTime > x.AvailableEndTime
                        && (time >= x.AvailableStartTime || time <= x.AvailableEndTime)))
                .ToListAsync();
        }
    }
}
