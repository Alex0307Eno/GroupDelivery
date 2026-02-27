using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class BatchCreateMenuRequest
    {
        public int StoreId { get; set; }

        public List<MenuItemDto> Items { get; set; }
    }

    public class MenuItemDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string ImageUrl { get; set; }

        public List<OptionGroupDto> OptionGroups { get; set; }
    }

    public class OptionGroupDto
    {
        public string GroupName { get; set; }

        public List<OptionDto> Options { get; set; }
    }

    public class OptionDto
    {
        public string OptionName { get; set; }

        public decimal PriceAdjust { get; set; }
    }


}
