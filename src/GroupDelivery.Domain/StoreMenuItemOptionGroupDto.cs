using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreMenuItemOptionGroupDto
    {
        public int StoreMenuItemOptionGroupId { get; set; }

        public string GroupName { get; set; }

        public List<StoreMenuItemOptionDto> Options { get; set; }
    }
}
