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

        // 最大配送距離
        [Required]
        public decimal MaxDistanceKm { get; set; }

        // 成團金額
        [Required]
        public decimal MinimumOrderAmount { get; set; }

        // 未達門檻外送費
        [Required]
        public decimal DeliveryFeeIfNotMet { get; set; }

        // 排序
        public int SortOrder { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
    }
}