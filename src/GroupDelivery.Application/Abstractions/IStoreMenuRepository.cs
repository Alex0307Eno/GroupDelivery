using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreMenuRepository
    {
        Task<List<StoreMenuItem>> GetByStoreIdAsync(int storeId);

        Task<StoreMenuItem> GetByIdAsync(int id);

        Task AddAsync(StoreMenuItem item);

        void Update(StoreMenuItem item);

        void Remove(StoreMenuItem item);


        Task SaveChangesAsync();
    }
}