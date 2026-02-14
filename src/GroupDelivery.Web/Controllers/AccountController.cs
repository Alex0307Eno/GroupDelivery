using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class AccountController : Controller
{
    private readonly IUserRepository _userRepo;
    private readonly IAuthService _authService;
    private readonly IMerchantService _merchantService;
    private readonly IUserService _userService;

    public AccountController(IUserRepository userRepo, IAuthService authService, IMerchantService merchantService, IUserService userService)
    {
        _userRepo = userRepo;
        _authService = authService;
        _merchantService = merchantService;
        _userService = userService;
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

    public IActionResult Orders()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ProfileData()
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        var profile = await _userService.GetProfileAsync(userId);

        if (profile == null)
            return Unauthorized();

        return Json(profile);
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
        if (User.FindFirst(ClaimTypes.Role)?.Value == nameof(UserRole.Merchant))
            return RedirectToAction("Profile");

        return View();
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(
    [FromBody] UpdateProfileRequest req)
    {
        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        await _userService.UpdateProfileAsync(userId, req);

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
    public async Task<IActionResult> CreateMerchant(
    [FromBody] MerchantInfoDto dto)
    {
        if (dto == null)
            return BadRequest("請求資料為空");

        var userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        var storeId = await _merchantService.CreateStoreAsync(userId, dto);

        return Ok(new { storeId });
    }
}
