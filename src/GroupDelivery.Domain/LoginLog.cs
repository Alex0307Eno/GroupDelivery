using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class LoginLog
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string LoginType { get; set; }
        public string IpAddress { get; set; }

        public string UserAgent { get; set; }

        public bool Success { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
