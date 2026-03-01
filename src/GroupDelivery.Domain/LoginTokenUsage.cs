using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class LoginTokenUsage
    {
        public int Id { get; set; }

        public string Nonce { get; set; }

        public DateTime ExpireAt { get; set; }

        public bool Used { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
