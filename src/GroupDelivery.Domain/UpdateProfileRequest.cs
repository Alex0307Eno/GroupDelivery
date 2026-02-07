using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class UpdateProfileRequest
    {
        public string Nickname { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Bio { get; set; }
        public string FoodPreference { get; set; }
        public bool NotifyOptIn { get; set; }
    }

}
