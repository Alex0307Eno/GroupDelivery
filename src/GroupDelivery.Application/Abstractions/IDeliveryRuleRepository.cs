using System.Collections.Generic;
using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Application.Abstractions
{
    public interface IDeliveryRuleRepository
    {
        Task<List<DeliveryRule>> GetByStoreIdAsync(int storeId);

        Task AddAsync(DeliveryRule rule);

        Task DeleteAsync(int id);

        Task SaveChangesAsync();
    }
}