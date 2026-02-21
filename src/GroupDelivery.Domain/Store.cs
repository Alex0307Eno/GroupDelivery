using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupDelivery.Domain
{
    [Table("tbStore")]
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        public int OwnerUserId { get; set; }

        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public string Description { get; set; }

        [StringLength(255)]
        public string CoverImageUrl { get; set; }

        [StringLength(255)]
        public string MenuImageUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        [NotMapped]
        public bool HasActiveGroupOrders { get; set; }
    }
}
