using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreMenuCategory
    {
        public int Id { get; set; }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public Store Store { get; set; }
    }

}
