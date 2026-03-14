using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreListItemViewModel
    {
        public int StoreId { get; set; }

        public Guid StorePublicId { get; set; }

        public string StoreName { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string Landline { get; set; }

        public string CoverImageUrl { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }

        public TimeSpan? OpenTime2 { get; set; }

        public TimeSpan? CloseTime2 { get; set; }

        public string ClosedDays { get; set; }

        public bool IsPausedToday { get; set; }

        public bool HasMenu { get; set; }

        public bool HasDeliveryRule { get; set; }

        public bool IsClosedToday { get; set; }

        public List<int> ClosedDayList { get; set; }

        public bool CanOpenGroup
        {
            get
            {
                return HasMenu && HasDeliveryRule && !IsPausedToday && !IsClosedToday;
            }
        }

        public bool HasPendingSetup
        {
            get
            {
                return !HasMenu || !HasDeliveryRule;
            }
        }
    }
}
