using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreNearbyDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string CoverImageUrl { get; set; }
        public string BusinessHours { get; set; }
        public bool IsOpenNow { get; set; }
        public bool HasActiveGroup { get; set; }
        public System.DateTime? ActiveGroupDeadline { get; set; }
        public double? Distance { get; set; }
    }
}
