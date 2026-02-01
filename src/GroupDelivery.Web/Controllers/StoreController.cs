using Microsoft.AspNetCore.Mvc;

namespace GroupDelivery.Web.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateGroup()
        {
            return View();
        }
    }
}
