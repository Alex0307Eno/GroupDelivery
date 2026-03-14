using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using GroupDelivery.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GroupDelivery.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly GroupDeliveryDbContext _db;
        private readonly IGroupService _groupService;
        private readonly IGroupOrderService _groupOrderService;
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;
        public GroupController(IGroupService groupService, IGroupOrderService groupOrderService, IOrderService orderService, IStoreService storeService,GroupDeliveryDbContext db )
        {
            _groupService = groupService;
            _groupOrderService = groupOrderService;
            _orderService = orderService;
            _storeService = storeService;
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Create(int storeId)
        {
            var store = await _storeService.GetByIdAsync(storeId);
            if (store == null)
                return NotFound();

            var vm = new CreateUserGroupRequest
            {
                StorePublicId = store.StoreId,
                TargetAmount = 0,
                Deadline = DateTime.Now.AddHours(2)
            };

            return View(vm); 
        }

        [Authorize]
        [HttpGet("/group/manage/{Publicid}")]
        public async Task<IActionResult> Manage(Guid publicid)
        {
            var groupOrder = await _groupService.GetByIdAsync(publicid);

            if (groupOrder == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (groupOrder.OwnerUserId != userId)
                return Forbid();

            var orders = await _orderService.GetOrdersByGroupAsync(publicid);

            var vm = new GroupManageViewModel
            {
                GroupOrder = groupOrder,
                Orders = orders
            };

            return View(vm);
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
        public async Task<IActionResult> GroupDetail(Guid id)
        {
            var group = await _db.GroupOrders
                .Include(x => x.Store)
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.GroupOrderPublicId == id);

            if (group == null)
                return NotFound();

            return View(group);
        }
        [HttpPost("/group/{id}/join")]
        [ValidateAntiForgeryToken]
        public IActionResult Join(Guid id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var userId = int.Parse(claim.Value);

            _groupService.JoinGroupAsync(userId, id);

            return Redirect($"/group/{id}");
        }
        [HttpGet]
        public IActionResult Menu(Guid id)
        {
            ViewData["GroupPublicId"] = id;
            return View();
        }
    }
}
