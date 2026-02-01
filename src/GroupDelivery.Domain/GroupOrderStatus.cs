using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public enum GroupOrderStatus
    {
        Draft = 0,     // 尚未開放
        Open = 1,      // 揪團中
        Closed = 2,    // 已截止（未成團）
        Success = 3,   // 成團
        Cancelled = 4  // 取消
    }
}
