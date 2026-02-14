using GroupDelivery.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Web.Controllers
{
    [Authorize(Roles = "Merchant")]
    public class StoreMenuController : Controller
    {
        private readonly IStoreMenuService _service;

        public StoreMenuController(IStoreMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Create(int storeId)
        {
            ViewBag.StoreId = storeId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int storeId, string name, decimal price, string description)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            await _service.CreateMenuItemAsync(userId, storeId, name, price, description);

            return RedirectToAction("Manage", new { storeId });
        }


        [HttpGet]
        public async Task<IActionResult> Manage(int storeId)
        {
            var menu = await _service.GetMenuAsync(storeId);
            return View(menu);
        }
        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            await _service.ToggleActiveAsync(id);
            return RedirectToAction("Manage");
        }
        [HttpPost("batch")]
        public async Task<IActionResult> BatchCreate([FromBody] BatchMenuRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _service.BatchCreateAsync(userId, request.StoreId, request.Items);

            return Ok();
        }

    }
}
