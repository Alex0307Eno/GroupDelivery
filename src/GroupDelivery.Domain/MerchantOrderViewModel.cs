using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class MerchantOrderViewModel
    {
        public List<IGrouping<int, Order>> TodayGroups { get; set; }
        public List<IGrouping<int, Order>> WeekGroups { get; set; }
        public List<IGrouping<int, Order>> MonthGroups { get; set; }
    }
}
