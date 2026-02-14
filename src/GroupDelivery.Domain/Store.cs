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

        // 店家擁有者
        public int OwnerUserId { get; set; }

        // ======================
        // 基本資訊
        // ======================

        [Required]
        [StringLength(100)]
        public string StoreName { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public string Description { get; set; }

        // ======================
        // 地理位置
        // ======================

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // ======================
        // 營業時間
        // ======================

        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }

        public decimal? MinOrderAmount { get; set; }

        // ======================
        // 狀態（核心重構）
        // ======================

        // 商家每日營運狀態
        public StoreOperationStatus OperationStatus { get; set; }

        // 平台帳號狀態
        public StoreAccountStatus AccountStatus { get; set; }

        // ======================
        // 圖片
        // ======================

        [StringLength(255)]
        public string CoverImageUrl { get; set; }

        [StringLength(255)]
        public string MenuImageUrl { get; set; }

        // ======================
        // 補充說明
        // ======================

        [StringLength(500)]
        public string Notice { get; set; }

        // ======================
        // 系統欄位
        // ======================

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        // ======================
        // 計算屬性（不存資料庫）
        // ======================

        [NotMapped]
        public bool IsOpenNow
        {
            get
            {
                if (OperationStatus != StoreOperationStatus.Open)
                    return false;

                if (!OpenTime.HasValue || !CloseTime.HasValue)
                    return false;

                var now = DateTime.Now.TimeOfDay;

                return now >= OpenTime.Value && now <= CloseTime.Value;
            }
        }

        // ======================
        // 關聯集合
        // ======================

        public ICollection<StoreClosedDate> ClosedDates { get; set; }
        public ICollection<StoreWeeklyClosedDay> WeeklyClosedDays { get; set; }
    }

    // 商家營運狀態（商家每天會切換）
    public enum StoreOperationStatus
    {
        Open = 1,      // 正常營業
        Paused = 2,    // 暫停接單
        Holiday = 3    // 今日休息
    }

    // 帳號狀態（平台控制）
    public enum StoreAccountStatus
    {
        Draft = 1,       // 草稿
        Active = 2,      // 正式上架
        Suspended = 3    // 被停權
    }
}
