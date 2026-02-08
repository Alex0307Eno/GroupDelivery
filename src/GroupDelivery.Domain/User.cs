using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GroupDelivery.Domain
{
    [Table("tbUser")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        // ===== 核心身分 =====
        [StringLength(100)]
        public string Email { get; set; }   

        public UserRole Role { get; set; }

        [StringLength(50)]
        public string DisplayName { get; set; }

        public string PictureUrl { get; set; }

        // ===== 登入來源（先保留 LINE） =====
        [StringLength(50)]
        public string LineUserId { get; set; }

        // ===== 共用 =====
        [StringLength(20)]
        public string Phone { get; set; }

        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        // ===== 其他個人資訊 =====
        public string Nickname { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public string FoodPreference { get; set; }
        public bool NotifyOptIn { get; set; }

    }

    public enum UserRole
    {
        None = 0,
        User = 1,
        Merchant = 2
    }

}
