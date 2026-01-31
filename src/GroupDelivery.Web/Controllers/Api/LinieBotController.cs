using Microsoft.AspNetCore.Mvc;

namespace GroupDelivery.Web.Controllers.Api
{
    public class LinieBotController : Controller
    {
        [HttpGet("/signin-line")]
        public ActionResult LineCallback(string code, string state)
        {
            return Content("我有進來，code=" + code);
        }

    }
}
