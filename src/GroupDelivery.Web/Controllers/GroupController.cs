using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;


namespace GroupDelivery.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IGroupOrderService _groupOrderService;
        private readonly IOrderService _orderService;
        public GroupController(IGroupService groupService, IGroupOrderService groupOrderService, IOrderService orderService)
        {
            _groupService = groupService;
            _groupOrderService = groupOrderService;
            _orderService = orderService;
        }

        [Authorize]
        [HttpGet("/group/{id}/manage")]
        public async Task<IActionResult> Manage(int id)
        {
            var groupOrder = await _groupService.GetByIdAsync(id);

            if (groupOrder == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (groupOrder.OwnerUserId != userId)
                return Forbid();

            var orders = await _orderService.GetOrdersByGroupAsync(id);

            var vm = new GroupManageViewModel
            {
                GroupOrder = groupOrder,
                Orders = orders
            };

            return View(vm);
        }


        [HttpGet("/group/{id}")]
        public IActionResult Details(int id)
        {
            var group = _groupService.GetById(id);

            if (group == null)
                return NotFound();

            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = int.Parse(claim.Value);

            ViewBag.IsOwner = group.OwnerUserId == userId;

            return View(group);
        }

        [Authorize]
        [HttpPost("api/group/{groupId}/close")]
        public async Task<IActionResult> Close(int groupId)
        {
            var userId = GetUserId();
            await _groupService.CloseAsync(groupId, userId);
            return Ok();
        }


        [Authorize]
        [HttpPost("/group/{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            if (!_groupService.IsOwner(id, userId))
                return Forbid();

            TempData["Message"] = "團已取消（模擬）";
            return Redirect("/Store/MyGroups");
        }
        private int GetUserId()
        {
            return int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier).Value
            );
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> JoinGroup(int groupOrderId, decimal amount)
        {
            var userId = GetUserId();

            try
            {
                await _groupOrderService.JoinGroupAsync(userId, groupOrderId, amount);
                return Ok(new { message = "加入成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        // 團購詳情頁面
        public IActionResult GroupDetail(int id)
        {
            return View(id);
        }
        [HttpPost("/group/{id}/join")]
        [ValidateAntiForgeryToken]
        public IActionResult Join(int id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = int.Parse(claim.Value);

            _groupService.JoinGroupAsync(userId, id);

            return Redirect($"/group/{id}");
        }
        [HttpGet]
        public IActionResult Menu(int id)
        {
            ViewData["GroupId"] = id;
            return View();
        }
    }
}
