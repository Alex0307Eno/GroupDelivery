using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupDelivery.Domain
{
    public class DeliveryRule
    {
        [Key]
        public int DeliveryRuleId { get; set; }

        [Required]
        public int StoreId { get; set; }

        [Required]
        public decimal MaxDistanceKm { get; set; }

        [Required]
        public decimal MinimumOrderAmount { get; set; }

        [Required]
        public decimal DeliveryFeeIfNotMet { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
    }
}