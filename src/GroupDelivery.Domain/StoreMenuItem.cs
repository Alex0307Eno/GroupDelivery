using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreMenuItem
    {
        public int StoreMenuItemId { get; set; }

        public int StoreId { get; set; }

        public int? CategoryId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public StoreMenuCategory Category { get; set; }
        public virtual ICollection<GroupOrderItem> GroupOrderItems { get; set; }

    }

}
