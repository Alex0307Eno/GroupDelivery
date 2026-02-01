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

        // LINE 登入
        [StringLength(50)]
        public string LineUserId { get; set; }

        [StringLength(50)]
        public string DisplayName { get; set; }

        public UserRole Role { get; set; }

        public string PictureUrl { get; set; }

        // 共用
        [StringLength(20)]
        public string Phone { get; set; }

        // ===== 商家專用（Role = Merchant 才會用） =====

        [StringLength(100)]
        public string StoreName { get; set; }

        [StringLength(200)]
        public string StoreAddress { get; set; }

        [StringLength(20)]
        public string StorePhone { get; set; }

        // 定位用
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public enum UserRole
    {
        None = 0,
        User = 1,
        Merchant = 2
    }

}
