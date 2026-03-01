using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupDelivery.Domain;
using GroupDelivery.Application.Abstractions;

namespace GroupDelivery.Application.Services
{
    public class DeliveryRuleService : IDeliveryRuleService
    {
        private readonly IDeliveryRuleRepository _repository;

        public DeliveryRuleService(IDeliveryRuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DeliveryRule>> GetByStoreIdAsync(int storeId)
        {
            return await _repository.GetByStoreIdAsync(storeId);
        }

        public async Task AddRuleAsync(int storeId, decimal maxDistance, decimal minAmount, decimal fee)
        {
            var rule = new DeliveryRule
            {
                StoreId = storeId,
                MaxDistanceKm = maxDistance,
                MinimumOrderAmount = minAmount,
                DeliveryFeeIfNotMet = fee
            };

            await _repository.AddAsync(rule);
        }

        public async Task DeleteRuleAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public decimal? CalculateFee(decimal distanceKm, decimal orderAmount, List<DeliveryRule> rules)
        {
            var rule = rules
                .OrderBy(r => r.MaxDistanceKm)
                .FirstOrDefault(r => distanceKm <= r.MaxDistanceKm);

            if (rule == null)
                return null;

            if (orderAmount >= rule.MinimumOrderAmount)
                return 0;

            return rule.DeliveryFeeIfNotMet;
        }
    }
}