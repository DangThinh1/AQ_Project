using Identity.Core.Common;
using Identity.Core.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Identity.Core.Models.Authentications;
using System;

namespace Identity.Core.Extensions
{
    public static class ControllerBaseExtension
    {
        [Obsolete]
        public static AuthenticateViewModel GetUserAuthenticateModel(this ControllerBase controllerBase) 
            => JwtTokenHelper.GetSignInProfile(controllerBase.User);

        [Obsolete]
        public static string GetUserName(this ControllerBase controllerBase)
            => JwtTokenHelper.GetClaimValue(controllerBase.User, ClaimConstant.Name);

        [Obsolete]
        public static string GetUserName(this ClaimsPrincipal user)
            => JwtTokenHelper.GetClaimValue(user, ClaimConstant.Name);

        [Obsolete]
        public static string GetUserToken(this ControllerBase controllerBase)
            => JwtTokenHelper.GetClaimValue(controllerBase.User, ClaimConstant.AccessToken);

        [Obsolete]
        public static string GetUserToken(this ClaimsPrincipal user)
            => JwtTokenHelper.GetClaimValue(user, ClaimConstant.AccessToken);

        [Obsolete]
        public static string GetUserUniqueId(this ControllerBase controllerBase)
            => JwtTokenHelper.GetClaimValue(controllerBase.User, ClaimConstant.UniqueId );

        [Obsolete]
        public static string GetUserUniqueId(this ClaimsPrincipal user)
            => JwtTokenHelper.GetClaimValue(user, ClaimConstant.UniqueId);

        [Obsolete]
        public static string GetUserEmail(this ControllerBase controllerBase)
            => JwtTokenHelper.GetClaimValue(controllerBase.User, ClaimConstant.Email);

        [Obsolete]
        public static string GetUserEmail(this ClaimsPrincipal user)
            => JwtTokenHelper.GetClaimValue(user, ClaimConstant.Email);

        [Obsolete]
        public static string GetUserRoleId(this ControllerBase controllerBase)
            => JwtTokenHelper.GetClaimValue(controllerBase.User, ClaimConstant.RoleId);

        [Obsolete]
        public static string GetUserMerchantFid(this ControllerBase controllerBase)
            => JwtTokenHelper.GetClaimValue(controllerBase.User, ClaimConstant.MerchantId);
    }
}
