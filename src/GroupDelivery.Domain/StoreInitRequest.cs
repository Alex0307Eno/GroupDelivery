using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace GroupDelivery.Domain
{
    public class StoreInitRequest
    {
        // ===== 基本資訊 =====
        public int StoreId { get; set; } // 編輯商店時會帶入
        [Required]
        public string StoreName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        // ===== 營業資訊（不等於成團規則） =====

        [Required]
        public TimeSpan OpenTime { get; set; }

        [Required]
        public TimeSpan CloseTime { get; set; }

        // 商家目前是否接受訂單（暫停營業用）
        public bool IsAcceptingOrders { get; set; }

        // 給消費者看的公告（例如：只接晚餐）
        public string Notice { get; set; }

        // ===== 圖片 =====

        public IFormFile CoverImage { get; set; }

        public IFormFile[] MenuImages { get; set; }
    }
}
