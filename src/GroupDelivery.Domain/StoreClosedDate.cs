using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreClosedDate
    {
        public int StoreClosedDateId { get; set; }

        public int StoreId { get; set; }

        public DateTime ClosedDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public Store Store { get; set; }
    }
}
