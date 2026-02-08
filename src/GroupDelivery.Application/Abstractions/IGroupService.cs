using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDelivery.Domain;


namespace GroupDelivery.Application.Abstractions
{
    public interface IGroupService
    {
        Group GetById(int groupId);
        bool IsOwner(int groupId, int userId);
        List<Group> GetMyGroups(int ownerUserId);
        int GetCurrentMemberCount(int groupId);

        int GetTargetMemberCount(int groupId);


    }

}
