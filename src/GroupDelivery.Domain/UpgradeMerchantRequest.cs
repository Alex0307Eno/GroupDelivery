using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class UpgradeMerchantRequest
    {
        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }

        [Required]
        [StringLength(20)]
        public string StorePhone { get; set; }

        [Required]
        [StringLength(255)]
        public string StoreAddress { get; set; }
    }
}
