using System;
using System.Collections.Generic;

namespace GroupDelivery.Domain
{
    public class CategoryReorderRequest
    {
        public int CategoryId { get; set; }
        public int SortOrder { get; set; }
    }

    public class CategoryActiveUpdateRequest
    {
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryTransferRequest
    {
        public int SourceCategoryId { get; set; }
        public int TargetCategoryId { get; set; }
    }

    public class StoreMenuManageViewModel
    {
        public int StoreId { get; set; }
        public bool? CategoryIsActiveFilter { get; set; }
        public List<StoreMenuCategory> Categories { get; set; }
        public List<StoreMenuItem> Items { get; set; }
    }
}
