using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class MerchantInfoDto
    {
        public int? StoreId { get; set; }

        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string Landline { get; set; }
        public string Mobile { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
    }

}
