using System;
using System.Collections.Generic;

namespace GroupDelivery.Domain
{
    public class CategoryReorderRequest
    {
        public int CategoryId { get; set; }
        public int SortOrder { get; set; }
    }

    public class CategoryActiveRequest
    {
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryTransferRequest
    {
        public int SourceCategoryId { get; set; }
        public int TargetCategoryId { get; set; }
    }

    public class MenuItemAvailableDto
    {
        public int StoreMenuItemId { get; set; }
        public int StoreId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan? AvailableStartTime { get; set; }
        public TimeSpan? AvailableEndTime { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ApiResponse Ok(object data = null)
        {
            return new ApiResponse
            {
                Success = true,
                Data = data
            };
        }

        public static ApiResponse Fail(string message)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message
            };
        }
    }
}
