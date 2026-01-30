namespace GroupDelivery.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        // 首頁：顯示目前所有熱門團購
        public IActionResult Index()
        {
            return View();
        }
        //附近商家
        public IActionResult NearbyStores()
        {
            return View();
        }
        public IActionResult StoreGroups()
        {
            return View();
        }

        // 團購詳情頁面
        public IActionResult GroupDetail(int id)
        {
            ViewData["GroupId"] = id;
            return View();
        }

        public  IActionResult CreateGroup()
         {
            return View();
        }
    }
}
