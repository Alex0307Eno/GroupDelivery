namespace GroupDelivery.Web.Controllers
{
    using GroupDelivery.Application.Abstractions;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IGroupOrderService _groupOrderService;
        public HomeController(IGroupOrderService groupOrderService)
        {
            _groupOrderService = groupOrderService;
        }

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
        
       


        

        
    }
}
