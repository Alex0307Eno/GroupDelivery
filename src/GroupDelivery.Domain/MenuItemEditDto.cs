using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class MenuItemEditDto
    {
        public int StoreMenuItemId { get; set; }             // 要修改哪一筆
        public int StoreId { get; set; }                 // 所屬店家
        public string Name { get; set; }        // 品名
        public int CategoryId { get; set; }     // 分類
        public Decimal Price { get; set; }          // 價格
        public string Description { get; set; } // 說明
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }      // 是否啟用
        public List<StoreMenuItemOptionGroupDto> OptionGroups { get; set; }

    }
}
