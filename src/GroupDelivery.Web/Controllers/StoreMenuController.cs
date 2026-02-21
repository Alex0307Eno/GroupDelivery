using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
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
        private readonly IStoreMenuService _storeMenuService;

        public StoreMenuController(IStoreMenuService storeMenuService)
        {
            _storeMenuService = storeMenuService;
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

            var items = await _storeMenuService.GetMenuAsync(storeId);

            return View(items);
        }

        
        [HttpGet("BatchCreate/{storeId}")]
        public IActionResult BatchCreate(int storeId)
        {
            if (storeId <= 0)
            {
                return NotFound();
            }

            
            return View(model: storeId);
        }



        
        public class ToggleMenuRequest
        {
            public int Id { get; set; }
        }

    }
}
