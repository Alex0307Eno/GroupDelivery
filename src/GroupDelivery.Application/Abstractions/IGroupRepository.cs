using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Application.Abstractions
{
    public interface IGroupRepository
    {
        // 團主的所有團
        Task<List<GroupOrder>> GetByOwnerAsync(int ownerUserId);

        // 單筆團單
        Task<GroupOrder> GetByIdAsync(int groupId);

        // 更新團單
        Task UpdateAsync(GroupOrder group);
    }


}
