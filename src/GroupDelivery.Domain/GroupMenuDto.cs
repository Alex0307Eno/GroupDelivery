using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class GroupMenuDto
    {
        public int GroupOrderId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }

        public List<GroupMenuItemDto> Items { get; set; }
    }

    public class GroupMenuItemDto
    {
        public int StoreMenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
