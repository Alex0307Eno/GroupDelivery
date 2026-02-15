using System;
using System.Security.Claims;

namespace GroupDelivery.Web.Services
{
    public class CurrentStoreContextService : ICurrentStoreContextService
    {
        private const string StoreIdClaimType = "StoreId";

        public int GetCurrentStoreId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(StoreIdClaimType);
            if (claim == null || !int.TryParse(claim.Value, out var storeId) || storeId <= 0)
                throw new Exception("登入資訊缺少 StoreId");

            return storeId;
        }

        public bool HasAccessToStore(ClaimsPrincipal user, int storeId)
        {
            return GetCurrentStoreId(user) == storeId;
        }
    }
}
