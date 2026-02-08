using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class Group
    {
        public int GroupId { get; set; }

        // 團主
        public int OwnerUserId { get; set; }

        // 團基本資訊
        public string Title { get; set; }

        // 狀態
        public GroupStatus Status { get; set; }

        // 時間
        public DateTime CreatedAt { get; set; }
    }
    public enum GroupStatus
    {
        Draft = 0,
        Active = 1,
        Closed = 2,
        Cancelled = 3
    }

}
