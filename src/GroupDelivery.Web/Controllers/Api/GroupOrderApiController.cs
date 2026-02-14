using GroupDelivery.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/groups")]
public class GroupOrderApiController : ControllerBase
{
    private readonly IGroupOrderService _groupOrderService;

    public GroupOrderApiController(IGroupOrderService groupOrderService)
    {
        _groupOrderService = groupOrderService;
    }

    // GET api/groups/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _groupOrderService.GetDetailAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    // POST api/groups/5/join
    [HttpPost("{id}/join")]
    public async Task<IActionResult> Join(int id)
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
            return Unauthorized();

        var userId = int.Parse(claim.Value);

        await _groupOrderService.JoinGroupAsync(userId, id);

        return Ok();
    }
}
