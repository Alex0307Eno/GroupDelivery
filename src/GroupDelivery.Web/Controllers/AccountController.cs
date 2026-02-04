using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
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
    private readonly IUserRepository _userRepo;
    private readonly IAuthService _authService;
    private readonly IMerchantService _merchantService;

    public AccountController(GroupDeliveryDbContext db, IUserRepository userRepo, IAuthService authService, IMerchantService merchantService)
    {
        _db = db;
        _userRepo = userRepo;
        _authService = authService;
        _merchantService = merchantService;
    }

    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        var user = await _userRepo.GetByIdAsync(userId);

        if (user == null)
            return Redirect("/Auth/Logout");

        return View(user);
    }
    [Authorize]
    public async Task<IActionResult> CompleteProfile()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        var user = await _userRepo.GetByIdAsync(userId);

        if (user == null)
            return Redirect("/Auth/Logout");

        return View(user);
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CompleteProfile(
    string displayName,
    string phone)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        var user = await _userRepo.GetByIdAsync(userId);

        if (user == null)
            return Redirect("/Auth/Logout");

        user.DisplayName = displayName;
        user.Phone = phone;

        await _db.SaveChangesAsync();

        return Redirect("/");
    }

    [Authorize]
    public async Task<IActionResult> AfterLogin()
    {
        if (!_authService.IsProfileCompleted(User))
            return Redirect("/Account/CompleteProfile");

        return Redirect("/");
    }
    [Authorize]
    [HttpGet]
    public IActionResult UpgradeToMerchant()
    {
        // 已是商家就不要再來
        if (User.FindFirst("Role")?.Value == nameof(UserRole.Merchant))
            return RedirectToAction("Profile");

        return View();
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
    [HttpPost]
    [Authorize] 
    public async Task<IActionResult> UpgradeToMerchant(
    [FromBody] UpgradeMerchantRequest request)
    {
        if (request == null)
            return BadRequest("請求資料為空");

        if (!User.Identity.IsAuthenticated)
            return Unauthorized("未登入");

        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (claim == null)
            return Unauthorized("登入狀態失效");

        int userId;
        if (!int.TryParse(claim.Value, out userId))
            return Unauthorized("使用者識別錯誤");

        await _merchantService.UpgradeToMerchant(userId, request);

        await _authService.RefreshSignInAsync(userId);

        return Ok(new { success = true });
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
