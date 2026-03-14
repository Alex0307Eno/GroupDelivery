//using GroupDelivery.Application.Abstractions;
//using GroupDelivery.Domain;
//using GroupDelivery.Infrastructure.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace GroupDelivery.Web.Controllers.Api
//{
//    [ApiController]
//    public class StoreBrowseApiController : ControllerBase
//    {
//        private readonly GroupDeliveryDbContext _db;
//        private readonly IStoreService _storeService;

//        public StoreBrowseApiController(GroupDeliveryDbContext db, IStoreService storeService)
//        {
//            _db = db;
//            _storeService = storeService;
//        }

//        //[HttpGet("/api/stores/nearby")]
//        public async Task<IActionResult> NearbyStores()
//        {
//            var result = await _storeService.GetNearbyStoresAsync();
//            return Ok(result);
//        }

        
        
//    }
//}
