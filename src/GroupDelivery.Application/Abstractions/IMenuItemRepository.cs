using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IMenuItemRepository
    {
        Task<List<MenuItem>> GetActiveByStoreAsync(int storeId, int? categoryId);
        Task<List<MenuItem>> GetDeletedByStoreAsync(int storeId);
        Task<MenuItem> GetByIdAsync(int id, int storeId);
        Task AddAsync(MenuItem item);
        Task UpdateAsync(MenuItem item);

        Task<List<Category>> GetCategoriesByStoreAsync(int storeId);
        Task<Category> GetCategoryByIdAsync(int categoryId, int storeId);
        Task<Category> GetCategoryByNameAsync(int storeId, string name);
        Task AddCategoryAsync(Category category);
    }
}
