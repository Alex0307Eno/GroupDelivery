using GroupDelivery.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [Route("api/stores")]
    [ApiController]
    public class StoreApiController : Controller
    {
        private readonly IStoreService _storeService;

        public StoreApiController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearby(double? lat, double? lng, string city)
        {
            var result = await _storeService.GetNearbyStoresAsync(lat, lng, city);
            return Ok(result);
        }
    }
}
