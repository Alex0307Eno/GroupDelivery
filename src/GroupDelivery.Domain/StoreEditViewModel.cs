using System.ComponentModel.DataAnnotations;

namespace GroupDelivery.Domain
{
    public class StoreEditViewModel
    {
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
    }
}
