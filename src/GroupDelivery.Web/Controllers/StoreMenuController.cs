using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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





        [HttpGet("Manage/{storeId}")]
        public async Task<IActionResult> Manage(int storeId)
        {
            ViewBag.StoreId = storeId; 

            var items = await _service.GetMenuAsync(storeId);

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            await _service.ToggleActiveAsync(id);
            return RedirectToAction("Manage");
        }
        [HttpGet("BatchCreate/{storeId}")]
        public IActionResult BatchCreate(int storeId)
        {
            ViewBag.StoreId = storeId;
            return View();
        }

        


    }
}
