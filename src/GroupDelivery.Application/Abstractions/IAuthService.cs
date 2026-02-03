using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IAuthService
    {
        Task SendLoginLinkAsync(string email);
        Task SignInByTokenAsync(string token, HttpContext httpContext);
        bool IsProfileCompleted(ClaimsPrincipal user);
        Task RefreshSignInAsync(int userId);
    }

}
