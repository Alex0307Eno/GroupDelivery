using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GroupDelivery.Application.Services
{
    public class GroupService : IGroupService
    {
        public Group GetById(int groupId)
        {
            return new Group
            {
                GroupId = groupId,
                OwnerUserId = 1, // 先假設是 1
                Title = "測試便當團",
                Status = GroupOrderStatus.Open,
                CreatedAt = DateTime.Now
            };
        }

        public bool IsOwner(int groupId, int userId)
        {
            var group = GetById(groupId);
            return group.OwnerUserId == userId;
        }

        public List<Group> GetMyGroups(int ownerUserId)
        {
            // 先假資料，等接 DB 再換
            return new List<Group>
        {
            new Group
            {
                GroupId = 1,
                OwnerUserId = ownerUserId,
                Title = "午餐便當團",
                Status = GroupOrderStatus.Cancelled,
                CreatedAt = DateTime.Now.AddHours(-1)
            },
            new Group
            {
                GroupId = 2,
                OwnerUserId = ownerUserId,
                Title = "下午茶揪團",
                Status = GroupOrderStatus.Expired,
                CreatedAt = DateTime.Now.AddDays(-1)
            }
        };
        }
        public int GetCurrentMemberCount(int groupId)
        {
            return 0;
        }

        public int GetTargetMemberCount(int groupId)
        {
            return 10;
        }

    }

}
