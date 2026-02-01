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

        // LINE 登入用，核心欄位
        [StringLength(50)]
        public string LineUserId { get; set; }

        // 顯示用，不強迫
        [StringLength(50)]
        public string DisplayName { get; set; }
        public UserRole Role { get; set; }
        public string PictureUrl { get; set; }



        // 手機登入或責任歸屬用
        [StringLength(20)]
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; }
    }
    public enum UserRole
    {
        None = 0,
        User = 1,
        Merchant = 2
    }

}
