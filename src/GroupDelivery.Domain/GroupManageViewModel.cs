using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class GroupManageViewModel
    {
        public GroupOrder GroupOrder { get; set; }
        public List<Order> Orders { get; set; }
    }
}
