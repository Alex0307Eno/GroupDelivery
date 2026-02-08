using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreEditViewModel
    {
        public int StoreId { get; set; }
        // 店家擁有者（系統指定，不由使用者輸入）
        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }
        // 聯絡電話
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        // 地址（可選）
        [StringLength(255)]
        public string Address { get; set; }
        // 營業時間（可選）
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        // 是否接單中（臨時關店用）
        public bool IsAcceptingOrders { get; set; }
        // 最低成團金額
        public decimal? MinOrderAmount { get; set; }

        public string Notice { get; set; }
    }

}
