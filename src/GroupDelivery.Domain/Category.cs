using System;
using System.Collections.Generic;

namespace GroupDelivery.Domain
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
