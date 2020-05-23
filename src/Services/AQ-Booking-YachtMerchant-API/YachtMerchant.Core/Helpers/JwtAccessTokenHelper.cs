using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace YachtMerchant.Core.Helpers
{
    public static  class JwtAccessTokenHelper
    {
        private static HttpContext Current => new HttpContextAccessor().HttpContext;

        public static string GetClaimValue(ClaimsPrincipal principal, string claimType)
        {
            return principal?.Identities?.FirstOrDefault()?.FindFirst(claimType)?.Value??string.Empty;
        }

        public  static void AddUpdateClaim( string key, string value)
        {
            var identity = Current?.User?.Identity as ClaimsIdentity;

            if (identity == null)
                return;
            // check for existing claim and remove it
            var existingClaim = identity.FindAll(key)?.ToList();

            if (existingClaim != null)
            {
                foreach (var item in existingClaim)
                {
                    identity.RemoveClaim(item);
                }
            }
            // add new claim
            identity.AddClaim(new Claim(key, value));
            Current.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(Current.User.Identity));
        }

        public static string GetClaimValue(string key)
        {
            var identity= Current?.User?.Identity as ClaimsIdentity;
            if (identity == null)
                return string.Empty;

            var claim = identity?.Claims?.FirstOrDefault(c => c.Type == key);

            return  claim?.Value??string.Empty;
        }
    }
}
