using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class GroupMenuCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<GroupMenuItemDto> Items { get; set; }
    }
}
