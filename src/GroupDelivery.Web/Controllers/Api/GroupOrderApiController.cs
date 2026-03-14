using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    // 團單 API，負責團單查詢與操作入口
    [ApiController]
    [Route("api/groups")]
    public class GroupOrderApiController : ControllerBase
    {
        private readonly IGroupOrderService _groupOrderService;

        public GroupOrderApiController(IGroupOrderService groupOrderService)
        {
            _groupOrderService = groupOrderService;
        }

        #region API Endpoints

        // 依團單公開識別碼取得團單詳情
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _groupOrderService.GetGroupDetailAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // 取得開團清單，可依座標回傳距離排序
        [HttpGet]
        public async Task<IActionResult> Get(double? lat, double? lng)
        {
            var result = await _groupOrderService.GetOpenGroupsAsync(lat, lng);
            return Ok(result);
        }

        // 加入團單，必須登入才可操作
        [Authorize]
        [HttpPost("{id}/join")]
        public async Task<IActionResult> Join(int id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = int.Parse(claim.Value);

            var group = await _groupOrderService.GetByIdAsync(id);

            if (group == null)
                return NotFound("團單不存在");

            if (group.Deadline <= DateTime.Now)
                return BadRequest("團單已截止");

            if (group.Status != GroupOrderStatus.Open)
                return BadRequest("團單已結束");

            await _groupOrderService.JoinGroupAsync(userId, id);

            return Ok();
        }

        // 取得指定團單菜單資料
        [HttpGet("{id}/menu")]
        public async Task<IActionResult> GetMenu(int id)
        {
            var dto = await _groupOrderService.GetMenuAsync(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        // 設定訂單取餐方式，必須登入且由訂單擁有者操作
        [Authorize]
        [HttpPost("take-mode")]
        public async Task<IActionResult> SetTakeMode(int groupOrderId, TakeMode takeMode)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = int.Parse(claim.Value);
            await _groupOrderService.SetTakeModeAsync(userId, groupOrderId, takeMode);
            return Ok();
        }

        // 關閉團單，必須登入，由 Service 驗證是否為團主
        [Authorize]
        [HttpPost("{id}/close")]
        public async Task<IActionResult> CloseGroup(int id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = int.Parse(claim.Value);

            await _groupOrderService.CloseGroupAsync(userId, id);

            return RedirectToAction("MerchantOrders", "Order", new { id = id });
        }

        #endregion
    }
}
