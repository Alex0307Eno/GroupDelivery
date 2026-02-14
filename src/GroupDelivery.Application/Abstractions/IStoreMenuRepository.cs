using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuRepository
    {
        Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId);

        Task<StoreMenuItem> GetByIdAsync(int id);

        Task AddAsync(StoreMenuItem item);

        Task UpdateAsync(StoreMenuItem item);
        Task SaveChangesAsync();

    }


}
