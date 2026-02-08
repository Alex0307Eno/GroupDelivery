using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupDelivery.Domain
{
    [Table("tbStore")]
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        

        // 店家擁有者（系統指定，不由使用者輸入）
        public int OwnerUserId { get; set; }

        // ======================
        // 基本資訊（使用者會填）
        // ======================

        // 店名
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
        public string Description { get; set; }

        // ======================
        // 地理位置
        // ======================

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // ======================
        // 營運資訊（第一階段）
        // ======================

        // 營業開始時間
        public TimeSpan? OpenTime { get; set; }

        // 營業結束時間
        public TimeSpan? CloseTime { get; set; }

        // 是否接單中（臨時關店用）
        public bool IsAcceptingOrders { get; set; }

        // 最低成團金額
        public decimal? MinOrderAmount { get; set; }
        public StoreOpenStatus CurrentStatus { get; set; }

        // ======================
        // 圖片（URL 即可，先別做太複雜）
        // ======================

        // 店家封面照
        [StringLength(255)]
        public string CoverImageUrl { get; set; }

        // 菜單圖片
        [StringLength(255)]
        public string MenuImageUrl { get; set; }

        // ======================
        // 補充說明
        // ======================

        // 店家備註（如：假日較忙、辣度可調）
        [StringLength(500)]
        public string Notice { get; set; }

        // ======================
        // 系統欄位
        // ======================

        // Draft / Active / Suspended
        [StringLength(20)]
        public string Status { get; set; }

        

        // 給前端用的即時計算結果
        public bool IsOpenNow { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        // 是否在放假中（由系統計算得出，非使用者輸入）
        public bool IsOnHoliday { get; set; }

        public DateTime? HolidayStartDate { get; set; }

        public DateTime? HolidayEndDate { get; set; }


        // ======================
        // 關聯集合
        // ======================


        public ICollection<StoreClosedDate> ClosedDates { get; set; }

        public ICollection<StoreWeeklyClosedDay> WeeklyClosedDays { get; set; }

    }

    public enum StoreOpenStatus
        {
            Open = 0,    // 營業中
            Closed = 1,  // 休息中
            Paused = 2   // 暫停接單
        }
    

}
