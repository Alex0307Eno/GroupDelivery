using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/merchant")]
    public class MerchantGroupController : ControllerBase
    {
        private readonly GroupOrderService _groupOrderService;

        public MerchantGroupController(GroupOrderService groupOrderService)
        {
            _groupOrderService = groupOrderService;
        }

        [HttpPost("groups")]
        public IActionResult CreateGroup([FromBody] CreateGroupRequest request)
        {
            if (request == null)
                return BadRequest("請求資料為空");

            if (request.TargetAmount <= 0)
                return BadRequest("成團金額不正確");

            if (request.Deadline <= DateTime.Now.AddMinutes(30))
                return BadRequest("截止時間需至少 30 分鐘後");

            // 暫時先不處理登入，之後再補
            var group = new GroupOrder
            {
                StoreId = request.StoreId,
                CreatorUserId = request.CreatorUserId,
                TargetAmount = request.TargetAmount,
                Deadline = request.Deadline,
                Remark = request.Remark
            };

            _groupOrderService.CreateGroupAsync(group).Wait();

            return Ok(new { message = "開團成功" });
        }
    }
}
