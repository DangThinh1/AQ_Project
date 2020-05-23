using Microsoft.AspNetCore.Identity;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Database.Entities;
using APIHelpers.Response;
using System;
using Identity.Core.Helpers;
using Identity.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Identity.Infrastructure.Services.Interfaces;
using Identity.Core.Models.Authentications;

namespace Identity.Infrastructure.Services.Implements
{
    public class UserTokenService : IdentityServiceBase, IUserTokenService
    {
        public UserTokenService(IdentityDbContext db, UserManager<Users> userManager) : base(db, userManager)
        {
        }

        public BaseResponse<string> Create(string token, string returnUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || JwtTokenHelper.DecodeJwtToken(token) == null)
                    return BaseResponse<string>.BadRequest();

                var authModel = JwtTokenHelper.DecodeJwtToken(token);
                var userToken = new UserTokens() {
                    Id = UniqueIDHelper.GenarateRandomString(12),
                    UserFid = Guid.Parse(authModel.UserId),
                    AccessToken = token,
                    ReturnUrl = returnUrl,
                };
                _db.UserTokens.Add(userToken);
                if(_db.SaveChanges() > 0)
                    return BaseResponse<string>.Success(userToken.Id);
                return BaseResponse<string>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<AuthenticateViewModel> FindUserToken(string id)
        {
            try
            {
                var entity = _db.UserTokens.AsNoTracking().FirstOrDefault(k=> k.Id.ToUpper() == id.ToUpper());
                if (entity != null)
                {
                    var authModel = JwtTokenHelper.DecodeJwtToken(entity.AccessToken);
                    authModel.AccessToken = entity.AccessToken;
                    return BaseResponse<AuthenticateViewModel>.Success(authModel);
                }
                   
                return BaseResponse<AuthenticateViewModel>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<AuthenticateViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteUserTokenByUserId(string uid)
        {
            try
            {
                if(string.IsNullOrEmpty(uid))
                    return BaseResponse<bool>.BadRequest();
                var entities = _db.UserTokens.AsNoTracking().Where(k => k.UserFid.ToString().ToUpper() == uid.ToUpper()).ToList();
                if (entities != null)
                {
                    _db.UserTokens.RemoveRange(entities);
                    if(_db.SaveChanges() > 0)
                    {
                        return BaseResponse<bool>.Success(true);
                    }
                }

                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
