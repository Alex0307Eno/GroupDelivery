using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class GroupMenuOptionGroupDto
    {
        public string GroupName { get; set; }

        public List<GroupMenuOptionDto> Options { get; set; }
    }
}
