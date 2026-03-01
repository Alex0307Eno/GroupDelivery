using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class CreateMerchantGroupRequest
    {
        public List<StoreDto> AvailableStores { get; set; }
    }
}
