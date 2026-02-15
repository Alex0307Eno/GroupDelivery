using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/storemenu")]
    [Authorize(Roles = "Merchant")]
    public class StoreMenuApiController : ControllerBase
    {
        private readonly IStoreMenuService _service;

        public StoreMenuApiController(IStoreMenuService service)
        {
            _service = service;
        }

        [HttpPost("batchcreate")]
        public async Task<IActionResult> BatchCreate()
        {
            var form = Request.Form;

            if (!form.ContainsKey("storeId"))
                return BadRequest("StoreId 缺失");

            int storeId = int.Parse(form["storeId"]);

            var itemsJson = form["items"];
            var items = System.Text.Json.JsonSerializer
                .Deserialize<List<MenuItemDto>>(itemsJson);

            if (storeId <= 0)
                return BadRequest("StoreId 錯誤");

            if (items == null || !items.Any())
                return BadRequest("沒有菜單項目");

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            await _service.BatchCreateAsync(userId, storeId, items);

            // 圖片處理
            var file = form.Files.FirstOrDefault();
            if (file != null)
            {
                var path = Path.Combine("wwwroot/uploads", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok();
        }

    }
}
