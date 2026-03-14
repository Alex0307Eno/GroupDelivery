using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
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
        private readonly IGroupService _groupService;

        public GroupOrderApiController(IGroupOrderService groupOrderService, IGroupService groupService)
        {
            _groupOrderService = groupOrderService;
            _groupService = groupService;
        }


        #region 依團單公開識別碼取得團單詳情
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _groupOrderService.GetGroupDetailAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        #endregion

        #region 取得開團清單，可依座標回傳距離排序
        [HttpGet]
        public async Task<IActionResult> Get(double? lat, double? lng)
        {
            var result = await _groupOrderService.GetOpenGroupsAsync(lat, lng);
            return Ok(result);
        }
        #endregion
        #region 加入團單，必須登入才可操作
        [Authorize]
        [HttpPost("join/{id}")]
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
        #endregion
        #region 取得指定團單菜單資料
        [HttpGet("menu/{publicId}")]
        public async Task<IActionResult> GetMenu(Guid publicId)
        {
            var dto = await _groupOrderService.GetMenuAsync(publicId);

            if (dto == null)
                return NotFound();

            return Ok(dto);
        }
        #endregion

        #region 設定訂單取餐方式，必須登入且由訂單擁有者操作
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
        #endregion

        #region 關閉團單，必須登入，由 Service 驗證是否為團主
        [Authorize]
        [HttpPost("close/{id}")]
        public async Task<IActionResult> CloseGroup(Guid id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = int.Parse(claim.Value);

            await _groupOrderService.CloseGroupAsync(userId, id);

            return Ok();
        }
        #endregion

        #region
        [HttpPost("cancel/{publicId}")]
        public async Task<IActionResult> Cancel(Guid publicId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return Unauthorized();

            int userId = int.Parse(claim.Value);

            await _groupService.CancelAsync(publicId, userId);

            return Ok();
        }
        #endregion
    }
}
