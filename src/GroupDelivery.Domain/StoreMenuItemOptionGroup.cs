using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreMenuItemOptionGroup
    {
        public int StoreMenuItemOptionGroupId { get; set; }

        public int StoreMenuItemId { get; set; }

        public string GroupName { get; set; }

        public virtual StoreMenuItem StoreMenuItem { get; set; }

        public virtual ICollection<StoreMenuItemOption> Options { get; set; }
    }
}
