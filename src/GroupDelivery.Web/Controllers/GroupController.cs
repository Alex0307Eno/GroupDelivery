using GroupDelivery.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace GroupDelivery.Web.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [Authorize]
        [HttpGet("/group/{id}/manage")]
        public async Task<IActionResult> Manage(int id)
        {
            var groupOrder = await _groupService.GetByIdAsync(id);
            return View(groupOrder);
        }

        [HttpGet("/group/{id}")]
        public IActionResult Details(int id)
        {
            var group = _groupService.GetById(id);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

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
    }
}
