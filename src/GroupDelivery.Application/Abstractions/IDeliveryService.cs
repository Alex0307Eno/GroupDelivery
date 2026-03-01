using System.Collections.Generic;
using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Application.Abstractions
{
    public interface IDeliveryRuleService
    {
        Task<List<DeliveryRule>> GetByStoreIdAsync(int storeId);

        Task AddRuleAsync(int storeId, decimal maxDistance, decimal minAmount, decimal fee);

        Task DeleteRuleAsync(int id);

        decimal? CalculateFee(decimal distanceKm, decimal orderAmount, List<DeliveryRule> rules);
    }
}