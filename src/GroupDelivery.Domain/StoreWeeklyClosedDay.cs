using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreWeeklyClosedDay
    {

        public int StoreWeeklyClosedDayId { get; set; }
        
        public int StoreId { get; set; }

        // 1 = Monday … 7 = Sunday
        public int DayOfWeek { get; set; }

        public DateTime CreatedAt { get; set; }

        public Store Store { get; set; }
    }

}
