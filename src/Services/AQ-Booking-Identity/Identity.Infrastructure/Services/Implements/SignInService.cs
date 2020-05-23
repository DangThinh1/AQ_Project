using System;
using APIHelpers.Response;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Database.Entities;
using Identity.Infrastructure.Services.Interfaces;

namespace Identity.Infrastructure.Services.Implements
{
    public class SignInService : IdentityServiceBase, ISignInService
    {
        public SignInService(IdentityDbContext db, UserManager<Users> manager) : base(db, manager)
        {
        }
        public BaseResponse<bool> IsAllowedToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return BaseResponse<bool>.BadRequest(); 

                var authModel = JwtTokenHelper.DecodeJwtToken(token);
                if (authModel != null)
                {
                    var user = Find(authModel.Email);
                    if (user != null)
                    {
                        var tokenEffectiveDate = long.Parse(authModel.TokenEffectiveTimeStick);
                        var userEffectiveDate = user.TokenEffectiveTimeStick;
                        if (tokenEffectiveDate > userEffectiveDate)
                            return BaseResponse<bool>.Success(true);
                        return BaseResponse<bool>.BadRequest(message: "Token is not allowed because user was signed out.");
                    }
                }

                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> SignOutAllDevice(string key)
        {
            try
            {
                var user = Find(key);
                if (user != null && UpdateEffectiveDateToken(user, DateTime.Now))
                    return BaseResponse<bool>.Success();
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
