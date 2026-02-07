using GroupDelivery.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IGroupOrderService
    {
        Task<List<GroupOrder>> GetActiveGroupsAsync();


        Task CreateGroupAsync(int userId, CreateGroupRequest request);

        Task<GroupDetailDto> GetGroupDetailAsync(int groupId);


        //Task RefreshGroupStatusAsync(int groupId);
    }
}
