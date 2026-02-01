using GroupDelivery.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Web.Controllers
{
    public class OnboardingController : Controller
    {
        public IActionResult ChooseRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetRole(UserRole role,
            [FromServices] GroupDeliveryDbContext db)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await db.Users.FindAsync(userId);

            user.Role = role;
            await db.SaveChangesAsync();

            return Redirect("/");
        }
    }
}
