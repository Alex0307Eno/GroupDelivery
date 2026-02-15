using System;
using System.Security.Claims;

namespace GroupDelivery.Web.Services
{
    public class CurrentUserContextService : ICurrentUserContextService
    {
        public int GetRequiredUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new Exception("登入資訊無效");

            return int.Parse(claim.Value);
        }
    }
}
