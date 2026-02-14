using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreMenu
    {
        public int StoreMenuId { get; set; }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        // 導覽屬性
        public virtual Store Store { get; set; }
    }
}
