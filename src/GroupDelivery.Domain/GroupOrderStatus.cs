using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public enum GroupOrderStatus
    {
        Draft = 0,      // 尚未開放
        Open = 1,       // 揪團中
        Expired = 2,    // 截止未成團（軟刪除）
        Success = 3,    // 成團
        Cancelled = 4   // 團主取消
    }

}
