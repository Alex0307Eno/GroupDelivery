using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class UserProfileDto
    {
        //使用者暱稱
        public string DisplayName { get; set; }
        //使用者頭像URL
        public string PictureUrl { get; set; }
        //手機號碼
        public string Phone { get; set; }
        //使用者角色（例如：一般會員、店家）
        public string Role { get; set; }
        //使用者所在城市（可選）
        public string City { get; set; }
        //使用者自我介紹（可選）
        public string Bio { get; set; }
        //使用者的食物偏好
        public string FoodPreference { get; set; }
        //使用者是否同意接收通知（例如：成團成功通知）
        public bool NotifyOptIn { get; set; }
    }

}
