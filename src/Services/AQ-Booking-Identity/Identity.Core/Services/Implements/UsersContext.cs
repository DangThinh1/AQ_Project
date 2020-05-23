using System;
using System.Linq;
using Identity.Core.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Identity.Core.Services.Interfaces;
using Identity.Core.Models.Authentications;

namespace Identity.Core.Services.Implements
{
    public class UsersContext : IUsersContext
    {
        private AuthenticateViewModel _userProfiles;
        private const string AUTHORIZATION_KEY = "Authorization";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContext HttpContext => _httpContextAccessor.HttpContext;
        public ClaimsIdentity UserClaims=> UserPrincipal.Identities.FirstOrDefault();
        public ClaimsPrincipal UserPrincipal => _httpContextAccessor.HttpContext.User;

        public AuthenticateViewModel UserProfiles => GetSignInProfile(UserClaims);

        public UsersContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Private Methods

        private void ThrowIfIHttpContextNull()
        {
            if (_httpContextAccessor == null)
                throw new Exception("IHttpContextAccessor is null, please inject IHttpContextAccessor interface into the startup.cs");
        }

        private void ThrowIfClaimsIdentityNull()
        {
            if (_httpContextAccessor == null)
                throw new Exception("ClaimsIdentity is null");
        }

        private void ThrowIfClaimsPrincipalNull()
        {
            if (_httpContextAccessor == null)
                throw new Exception("ClaimsPrincipal is null");
        }

        private AuthenticateViewModel GetSignInProfile(ClaimsIdentity claims)
        {
            try
            {
                if (claims == null)
                    return null;
                var profile = new AuthenticateViewModel()
                {
                    AccountTypeFid = GetClaimValue(claims, ClaimConstant.AccountType) ?? string.Empty,
                    UserId = GetClaimValue(claims, ClaimConstant.UserId) ?? string.Empty,
                    RoleId = GetClaimValue(claims, ClaimConstant.RoleId) ?? string.Empty,
                    UniqueId = GetClaimValue(claims, ClaimConstant.UniqueId) ?? string.Empty,
                    Name = GetClaimValue(claims, ClaimConstant.Name) ?? string.Empty,
                    Email = GetClaimValue(claims, ClaimConstant.Email) ?? string.Empty,
                    UserRole = GetClaimValue(claims, ClaimConstant.Role) ?? string.Empty,
                    AccessToken = GetClaimValue(claims, ClaimConstant.AccessToken) ?? string.Empty,
                    RefreshToken = GetClaimValue(claims, ClaimConstant.RefreshToken) ?? string.Empty,
                    TokenEffectiveDate = GetClaimValue(claims, ClaimConstant.TokenEffectiveDate) ?? string.Empty,
                    Expired = GetClaimValue(claims, ClaimConstant.TokenExpired) ?? string.Empty,
                    TokenEffectiveTimeStick = GetClaimValue(claims, ClaimConstant.TokenEffectiveTimeStick) ?? string.Empty
                };
                if(profile != null && string.IsNullOrEmpty(profile.AccessToken))
                {
                    profile.AccessToken = GetJwtTokenFromRequestHeader();
                }
                
                return profile;
            }
            catch
            {
                return null;
            }
        }

        private string GetClaimValue(ClaimsIdentity claimsIdentity, string claimType)
        {
            try
            {
                return claimsIdentity.FindFirst(claimType).Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetJwtTokenFromRequestHeader()
        {
            try
            {
                var tokenFromRequestHeader = HttpContext.Request.Headers[AUTHORIZATION_KEY].ToString() ?? string.Empty;
                if(!string.IsNullOrEmpty(tokenFromRequestHeader))
                {
                    tokenFromRequestHeader = tokenFromRequestHeader.Split(" ")[1];
                }
                return tokenFromRequestHeader;
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion Private Methods
    }
}