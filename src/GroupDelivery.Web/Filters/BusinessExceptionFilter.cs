using GroupDelivery.Application.Exceptions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GroupDelivery.Web.Filters
{
    public class BusinessExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BusinessException ex)
            {
                context.Result = new BadRequestObjectResult(ApiResponse.Fail(ex.Message));
                context.ExceptionHandled = true;
            }
        }
    }
}
