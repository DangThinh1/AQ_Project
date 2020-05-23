using YachtMerchant.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace YachtMerchant.Core.Extension
{
    public static class ControllerBaseExtentsion
    {
        //public static bool IsAdminOrDeveloper(this ControllerBase controller)
        //{
        //    return IsAdmin(controller) || IsDeveloper(controller);
        //}

        //public static bool IsAdmin(this ControllerBase controller)
        //{
        //    return JwtAccessTokenHelper.GetClaimValue(controller.User, ClaimTypes.Role).Equals("AQAdmin");
        //}

        //public static bool IsDeveloper(this ControllerBase controller)
        //{
        //    return JwtAccessTokenHelper.GetClaimValue(controller.User, ClaimTypes.Role).Equals("Developer");
        //}

        //public static bool IsEndUser(this ControllerBase controller)
        //{
        //    return JwtAccessTokenHelper.GetClaimValue(controller.User, ClaimTypes.Role).Equals("EndUser");
        //}

        //public static bool IsAQAccounting(this ControllerBase controller)
        //{
        //    return JwtAccessTokenHelper.GetClaimValue(controller.User, ClaimTypes.Role).Equals("AQAccounting");
        //}


    }
}
