using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Identity.Core.Models.Authentications;

namespace Identity.Core.Services.Interfaces
{
    public interface IUsersContext
    {
        HttpContext HttpContext { get; }
        ClaimsIdentity UserClaims { get; }
        ClaimsPrincipal UserPrincipal { get; }
        AuthenticateViewModel UserProfiles { get; }
    }
}
