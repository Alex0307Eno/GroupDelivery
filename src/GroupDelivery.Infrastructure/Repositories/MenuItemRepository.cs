using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly GroupDeliveryDbContext _dbContext;

        public MenuItemRepository(GroupDeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MenuItem>> GetActiveByStoreAsync(int storeId, int? categoryId)
        {
            var query = _dbContext.MenuItems
                .Include(x => x.Category)
                .Where(x => x.StoreId == storeId && !x.IsDeleted);

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            return await query
                .OrderBy(x => x.CategoryId)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<MenuItem>> GetDeletedByStoreAsync(int storeId)
        {
            return await _dbContext.MenuItems
                .Include(x => x.Category)
                .Where(x => x.StoreId == storeId && x.IsDeleted)
                .OrderByDescending(x => x.DeletedAt)
                .ToListAsync();
        }

        public async Task<MenuItem> GetByIdAsync(int id, int storeId)
        {
            return await _dbContext.MenuItems
                .FirstOrDefaultAsync(x => x.Id == id && x.StoreId == storeId);
        }

        public async Task AddAsync(MenuItem item)
        {
            _dbContext.MenuItems.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(MenuItem item)
        {
            _dbContext.MenuItems.Update(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Category>> GetCategoriesByStoreAsync(int storeId)
        {
            return await _dbContext.Categories
                .Where(x => x.StoreId == storeId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId, int storeId)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.StoreId == storeId);
        }

        public async Task<Category> GetCategoryByNameAsync(int storeId, string name)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.StoreId == storeId && x.Name == name);
        }

        public async Task AddCategoryAsync(Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
