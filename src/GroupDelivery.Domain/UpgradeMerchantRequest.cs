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
        // 商店名稱
        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }
        // 聯絡電話
        [Required]
        [StringLength(20)]
        public string StorePhone { get; set; }
        // 地址（可選）
        [Required]
        [StringLength(255)]
        public string StoreAddress { get; set; }
    }
}
