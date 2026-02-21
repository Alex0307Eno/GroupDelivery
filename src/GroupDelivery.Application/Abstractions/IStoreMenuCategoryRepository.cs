using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuCategoryRepository
    {
        Task AddAsync(StoreMenuCategory entity);
        Task<List<StoreMenuCategory>> GetByStoreIdAsync(int storeId);
        Task SaveChangesAsync();
    }

}
