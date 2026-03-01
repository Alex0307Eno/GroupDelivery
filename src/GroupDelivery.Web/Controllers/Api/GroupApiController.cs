using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
[Route("api/groups")]
[ApiController]
public class GroupApiController : ControllerBase
{
    private readonly IGroupOrderService _groupOrderService;

    public GroupApiController(IGroupOrderService groupOrderService)
    {
        _groupOrderService = groupOrderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserGroupRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var groupId = await _groupOrderService.CreateAsync(request, userId);

        return Ok(new { groupOrderId = groupId });
    }
}