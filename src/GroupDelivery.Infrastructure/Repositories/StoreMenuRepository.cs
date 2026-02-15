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

        public async Task AddAsync(StoreMenuItem item)
        {
            _db.StoreMenuItems.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuItems
                .Where(x => x.StoreId == storeId && x.IsActive)
                .OrderBy(x => x.CategoryId)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<StoreMenuItem>> GetAllByStoreIdAsync(int storeId)
        {
            return await _db.StoreMenuItems
                .Where(x => x.StoreId == storeId)
                .ToListAsync();
        }

        public async Task<List<StoreMenuItem>> GetAvailableMenuItemsAsync(int storeId, TimeSpan atTime)
        {
            return await _db.StoreMenuItems
                .Where(x => x.StoreId == storeId && x.IsActive)
                .Where(x =>
                    (x.AvailableStartTime == null && x.AvailableEndTime == null) ||
                    (x.AvailableStartTime != null && x.AvailableEndTime != null &&
                        (
                            (x.AvailableStartTime <= x.AvailableEndTime && atTime >= x.AvailableStartTime && atTime <= x.AvailableEndTime) ||
                            (x.AvailableStartTime > x.AvailableEndTime && (atTime >= x.AvailableStartTime || atTime <= x.AvailableEndTime))
                        )
                    ))
                .OrderBy(x => x.CategoryId)
                .ThenBy(x => x.Name)
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

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidOperationException("資料已被其他人更新，請重新整理後再試一次。");
            }
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<List<StoreMenuCategory>> GetCategoriesByStoreAsync(int storeId, bool? isActive)
        {
            var query = _db.StoreMenuCategories
                .Where(x => x.StoreId == storeId && !x.IsDeleted);

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            return await query
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<StoreMenuCategory>> GetCategoriesByIdsAsync(int storeId, List<int> categoryIds)
        {
            return await _db.StoreMenuCategories
                .Where(x => x.StoreId == storeId && !x.IsDeleted && categoryIds.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<StoreMenuCategory> GetCategoryByIdAsync(int categoryId)
        {
            return await _db.StoreMenuCategories
                .FirstOrDefaultAsync(x => x.Id == categoryId && !x.IsDeleted);
        }

        public async Task<bool> CategoryNameExistsAsync(int storeId, string name)
        {
            return await _db.StoreMenuCategories
                .AnyAsync(x => x.StoreId == storeId && !x.IsDeleted && x.Name == name);
        }

        public async Task AddCategoryAsync(StoreMenuCategory category)
        {
            await _db.StoreMenuCategories.AddAsync(category);
        }

        public async Task<bool> ExistsMenuItemByCategoryAsync(int categoryId)
        {
            return await _db.StoreMenuItems.AnyAsync(x => x.CategoryId == categoryId);
        }

        public void RemoveCategory(StoreMenuCategory category)
        {
            _db.StoreMenuCategories.Remove(category);
        }

        public async Task ReorderCategoriesAsync(int storeId, List<CategoryReorderRequest> requests)
        {
            using (var tx = await _db.Database.BeginTransactionAsync())
            {
                var categoryIds = requests.Select(x => x.CategoryId).ToList();
                var categories = await _db.StoreMenuCategories
                    .Where(x => x.StoreId == storeId && !x.IsDeleted && categoryIds.Contains(x.Id))
                    .ToListAsync();

                foreach (var request in requests)
                {
                    var category = categories.FirstOrDefault(x => x.Id == request.CategoryId);
                    if (category == null)
                        throw new InvalidOperationException("排序資料包含非本店分類");

                    category.SortOrder = request.SortOrder;
                }

                await _db.SaveChangesAsync();
                tx.Commit();
            }
        }

        public async Task ToggleCategoryActiveAsync(int storeId, List<CategoryActiveUpdateRequest> requests)
        {
            using (var tx = await _db.Database.BeginTransactionAsync())
            {
                var categoryIds = requests.Select(x => x.CategoryId).ToList();
                var categories = await _db.StoreMenuCategories
                    .Where(x => x.StoreId == storeId && !x.IsDeleted && categoryIds.Contains(x.Id))
                    .ToListAsync();

                foreach (var request in requests)
                {
                    var category = categories.FirstOrDefault(x => x.Id == request.CategoryId);
                    if (category == null)
                        throw new InvalidOperationException("啟用停用資料包含非本店分類");

                    category.IsActive = request.IsActive;
                }

                await _db.SaveChangesAsync();
                tx.Commit();
            }
        }

        public async Task<int> TransferCategoryItemsAsync(int storeId, int sourceCategoryId, int targetCategoryId)
        {
            using (var tx = await _db.Database.BeginTransactionAsync())
            {
                var items = await _db.StoreMenuItems
                    .Where(x => x.StoreId == storeId && x.CategoryId == sourceCategoryId)
                    .ToListAsync();

                foreach (var item in items)
                {
                    item.CategoryId = targetCategoryId;
                }

                await _db.SaveChangesAsync();
                tx.Commit();
                return items.Count;
            }
        }
    }
}
