using GroupDelivery.Domain;
using System;
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
        Task<StoreMenuItem> GetByPublicIdAsync(Guid publicId);
        Task<StoreMenuItem> GetWithOptionsAsync(int id);
        Task<List<StoreMenuItem>> GetByStoreIdWithOptionsAsync(int storeId,bool includeInactive);
        Task SaveChangesAsync();
        Task<bool> AnyActiveByStoreIdAsync(int storeId);
        Task<bool> AnyByStoreIdAsync(int storeId);

    }
}