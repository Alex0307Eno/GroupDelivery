using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreMenuItemOption
    {
        public int StoreMenuItemOptionId { get; set; }

        public int OptionGroupId { get; set; }

        public string OptionName { get; set; }

        public decimal PriceAdjust { get; set; }

        public virtual StoreMenuItemOptionGroup OptionGroup { get; set; }
    }
}
