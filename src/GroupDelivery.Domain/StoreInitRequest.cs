using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GroupDelivery.Domain
{
    public class StoreInitRequest
    {
        [Required]
        public string StoreName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public IFormFile CoverImage { get; set; }

        public IFormFile[] MenuImages { get; set; }
    }
}
