using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IMenuItemService
    {
        Task<List<MenuItem>> GetActiveAsync(int ownerUserId, int? categoryId);
        Task<List<MenuItem>> GetDeletedAsync(int ownerUserId);
        Task<MenuItem> GetByIdAsync(int ownerUserId, int id);
        Task CreateAsync(int ownerUserId, MenuItem item);
        Task UpdateAsync(int ownerUserId, MenuItem item);
        Task DeleteAsync(int ownerUserId, int id);
        Task RestoreAsync(int ownerUserId, int id);

        Task<List<Category>> GetCategoriesAsync(int ownerUserId);
        Task CreateCategoryAsync(int ownerUserId, string name);
    }
}
