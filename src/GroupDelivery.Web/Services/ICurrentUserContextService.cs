using System.Security.Claims;

namespace GroupDelivery.Web.Services
{
    public interface ICurrentUserContextService
    {
        int GetRequiredUserId(ClaimsPrincipal user);
    }
}
