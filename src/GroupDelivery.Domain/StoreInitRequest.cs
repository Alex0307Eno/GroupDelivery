using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GroupDelivery.Domain
{
    public class StoreInitRequest
    {
        [Required]
        public string StoreName { get; set; }

        [Required]
        public string Landline { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public IFormFile CoverImage { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }
        public TimeSpan? OpenTime2 { get; set; }
        public TimeSpan? CloseTime2 { get; set; }
        public List<int> ClosedDays { get; set; }
    }
}
