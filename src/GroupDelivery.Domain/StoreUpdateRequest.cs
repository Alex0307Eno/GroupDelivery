using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string Landline { get; set; }

        public string Mobile { get; set; }
        [StringLength(255)]
        public string Address { get; set; }

        public string Description { get; set; }

        public IFormFile? NewCoverImage { get; set; }

        public string? CurrentCoverImageUrl { get; set; }
        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }
        public TimeSpan? OpenTime2 { get; set; }

        public TimeSpan? CloseTime2 { get; set; }
        

        public List<int> ClosedDays { get; set; }
        public bool IsPausedToday { get; set; }
    }

}

