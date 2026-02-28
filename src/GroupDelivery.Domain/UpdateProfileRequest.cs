using System.ComponentModel.DataAnnotations;

namespace GroupDelivery.Domain
{
    public class UpdateProfileRequest
    {
        // 使用暱稱（可選）
        [StringLength(50)]
        public string Nickname { get; set; }

        // 聯絡電話（必填）
        [Required]
        [RegularExpression(@"^09\d{8}$",
            ErrorMessage = "手機格式必須為 09xxxxxxxx")]
        public string Phone { get; set; }

        // 居住地址（必填）
       

        // 居住城市（可選）
        [StringLength(50)]
        public string City { get; set; }

        // 自我介紹（可選）
        [StringLength(300)]
        public string Bio { get; set; }

        // 食物偏好（可選）
        [StringLength(200)]
        public string FoodPreference { get; set; }

        // 是否同意接收通知
        public bool NotifyOptIn { get; set; }
    }
}