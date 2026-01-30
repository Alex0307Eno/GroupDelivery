using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IGroupOrderRepository
    {
        Task<List<GroupOrder>> GetAllActiveAsync();
        Task<GroupOrder> GetByIdAsync(int id);
        Task AddAsync(GroupOrder group);
        Task UpdateAsync(GroupOrder group);
    }
}
