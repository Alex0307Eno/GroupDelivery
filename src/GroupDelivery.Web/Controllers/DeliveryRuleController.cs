using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GroupDelivery.Application.Abstractions;

namespace GroupDelivery.Web.Controllers
{
    public class DeliveryRuleController : Controller
    {
        private readonly IDeliveryRuleService _service;

        public DeliveryRuleController(IDeliveryRuleService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int id)
        {
            var rules = await _service.GetByStoreIdAsync(id);
            ViewBag.StoreId = id;
            return View(rules);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int storeId, decimal maxDistance, decimal minAmount, decimal fee)
        {
            await _service.AddRuleAsync(storeId, maxDistance, minAmount, fee);
            return RedirectToAction("Index", new { id = storeId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int storeId)
        {
            await _service.DeleteRuleAsync(id);
            return RedirectToAction("Index", new { id = storeId });
        }
    }
}