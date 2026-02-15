using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class BatchMenuRequest
    {
        public int StoreId { get; set; }
        public List<MenuItemDto> Items { get; set; }
    }

    public class MenuItemDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public TimeSpan? AvailableStartTime { get; set; }
        public TimeSpan? AvailableEndTime { get; set; }
    }

}
