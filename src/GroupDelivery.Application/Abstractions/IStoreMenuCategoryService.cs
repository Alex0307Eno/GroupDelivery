using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuCategoryService
    {
        Task CreateAsync(int userId, int storeId, string name);
        Task<List<StoreMenuCategory>> GetByStoreIdAsync(int storeId);
    }

}
