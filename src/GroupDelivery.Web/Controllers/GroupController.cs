using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GroupDelivery.Application.Abstractions;


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
        public IActionResult Manage(int id)
        {
            var userId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value
            );

            //if (!_groupService.IsOwner(id, userId))
            //    return Forbid();

            var group = _groupService.GetById(id);

            return View(group);
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
        [HttpPost("/group/{id}/close")]
        public IActionResult Close(int id)
        {
            // 先只驗證身分，不改資料
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            if (!_groupService.IsOwner(id, userId))
                return Forbid();

            // TODO: 之後才真的結團
            TempData["Message"] = "團已提前結束（模擬）";
            return Redirect($"/group/{id}/manage");
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

    }
}
