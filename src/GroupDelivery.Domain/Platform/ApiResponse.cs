namespace GroupDelivery.Domain.Platform
{
    public class ApiResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ApiResponse Success(string message, object data)
        {
            return new ApiResponse
            {
                Status = "Success",
                Message = message,
                Data = data
            };
        }

        public static ApiResponse Fail(string message)
        {
            return new ApiResponse
            {
                Status = "Fail",
                Message = message,
                Data = null
            };
        }
    }
}
