using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreUpdateRequest
    {
        [Required]
        public int StoreId { get; set; }

        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }
        public string Description { get; set; }


        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }

        public bool IsAcceptingOrders { get; set; }
        public decimal? MinOrderAmount { get; set; }

        public string Notice { get; set; }

        public IFormFile NewCoverImage { get; set; }
        public IFormFile[] NewMenuImages { get; set; }
    }
}
