using GroupDelivery.Infrastructure.Data;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class AccountController : Controller
{
    private readonly GroupDeliveryDbContext _db;

    public AccountController(GroupDeliveryDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var user = await _db.Users
            .FirstAsync(x => x.UserId == userId);

        return View(user);
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ProfileData()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var user = await _db.Users
            .Where(x => x.UserId == userId)
            .Select(x => new
            {
                displayName = x.DisplayName,
                lineUserId = x.LineUserId,
                pictureUrl = x.PictureUrl,
                role = x.Role.ToString()
            })
            .FirstAsync();

        return Json(user);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdatePhone(
    [FromBody] UpdatePhoneRequest req)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var user = await _db.Users.FindAsync(userId);
        user.Phone = req.Phone;

        await _db.SaveChangesAsync();
        return Ok();
    }

    public class UpdatePhoneRequest
    {
        public string Phone { get; set; }
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateMerchantInfo(
        [FromBody] MerchantInfoDto dto,
        [FromServices] GroupDeliveryDbContext db)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var user = await db.Users.FindAsync(userId);

        if (user == null)
            return Unauthorized();

        user.StoreName = dto.StoreName;
        user.StoreAddress = dto.StoreAddress;
        user.StorePhone = dto.StorePhone;
        user.Lat = dto.Lat;
        user.Lng = dto.Lng;

        await db.SaveChangesAsync();
        return Ok();
    }


}
