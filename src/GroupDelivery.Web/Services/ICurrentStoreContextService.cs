using System.Security.Claims;

namespace GroupDelivery.Web.Services
{
    public interface ICurrentStoreContextService
    {
        int GetCurrentStoreId(ClaimsPrincipal user);
        bool HasAccessToStore(ClaimsPrincipal user, int storeId);
    }
}
