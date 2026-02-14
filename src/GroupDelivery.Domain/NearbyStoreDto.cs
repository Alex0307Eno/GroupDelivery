using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class NearbyStoreDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public decimal Distance { get; set; }
        public bool HasActiveGroup { get; set; }
        public DateTime? ActiveGroupDeadline { get; set; }
        public DateTime? ActiveGroupCreatedAt { get; set; }
        public string CoverImageUrl { get; set; }
    }

}
