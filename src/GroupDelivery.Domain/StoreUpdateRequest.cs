using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class StoreUpdateRequest
    {
        [Required]
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
        // 給消費者看的店家介紹（可選）
        public string Description { get; set; }
        //營業時間
        public string BusinessTimePreset { get; set; }

        // 營業開始時間
        public TimeSpan? OpenTime { get; set; }
        // 營業結束時間
        public TimeSpan? CloseTime { get; set; }
        //是否接受接單
        public bool IsAcceptingOrders { get; set; }
        // 最低成團金額
        public decimal? MinOrderAmount { get; set; }
        // 給消費者看的公告（例如：只接晚餐）
        public string Notice { get; set; }
        // 商店和菜單圖片
        public IFormFile NewCoverImage { get; set; }
        public IFormFile[] NewMenuImages { get; set; }
        // 每週固定公休（0=週日、1=週一...6=週六）
        public List<StoreClosedDate> ClosedDates { get; set; }
        public StoreOperationStatus OperationStatus { get; set; }

        public bool IsOnHoliday { get; set; }

        // 可選，先留著未來用
        public DateTime? HolidayStartDate { get; set; }
        public DateTime? HolidayEndDate { get; set; }


    }
}
