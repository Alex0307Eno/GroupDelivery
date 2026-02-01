using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreInitRequest
    {
        [Required]
        public string StoreName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Address { get; set; }
    }

}
