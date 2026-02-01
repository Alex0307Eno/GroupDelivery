using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IGroupOrderService
    {
        Task<List<GroupOrder>> GetActiveGroupsAsync();

        Task<GroupOrder> GetGroupDetailAsync(int id);

        Task CreateGroupAsync(int userId, CreateGroupRequest request);

        //Task RefreshGroupStatusAsync(int groupId);
    }
}
