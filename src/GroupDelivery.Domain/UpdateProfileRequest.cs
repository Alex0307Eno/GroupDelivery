using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class UpdateProfileRequest
    {
        // 使用暱稱
        public string Nickname { get; set; }
        // 聯絡電話
        public string Phone { get; set; }
        // 居住城市
        public string City { get; set; }
        // 給其他使用者看的自我介紹（可選）
        public string Bio { get; set; }
        // 食物偏好
        public string FoodPreference { get; set; }
        // 是否同意接收通知（例如：成團成功、訂單狀態更新等）
        public bool NotifyOptIn { get; set; }
    }

}
