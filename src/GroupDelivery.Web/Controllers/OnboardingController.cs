using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers
{
    public class OnboardingController : Controller
    {
        public IActionResult ChooseRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetRole(
    string role,
    [FromServices] GroupDeliveryDbContext db)
        {
            var userId = int.Parse(
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var user = await db.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized();

            if (!Enum.TryParse<UserRole>(role, out var parsedRole))
                return BadRequest("Invalid role");

            user.Role = parsedRole;
            await db.SaveChangesAsync();

            if (user.Role == UserRole.Merchant)
                return Redirect("/Store/CreateGroup");

            return Redirect("/");
        }

    }
}
