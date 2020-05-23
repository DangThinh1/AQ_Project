using System;
using APIHelpers.Response;
using Identity.Core.Helpers;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Identity.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Identity.Core.Models.Authentications;
using Microsoft.AspNetCore.Authentication.Cookies;
//using AQEncrypts;
using Identity.Core.Models.Usertokens;

namespace Identity.Core.Services.Implements
{
    public class SignInRequestService : RequestServiceBase, ISignInRequestService
    { 
        private  string AuthenticationScheme { get; set; } = CookieAuthenticationDefaults.AuthenticationScheme;
        private string SSOAuthenticationScheme { get; set; } = CookieAuthenticationDefaults.AuthenticationScheme;
                
        public SignInRequestService(IUsersContext usersContext) : base(usersContext)
        {
        }

        public BaseResponse<string> CreateUserToken(string accessToken, string returnUrl, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? "api/SignIn/UserToken" : api,
                    RequestBody = new UserTokenCreateModel() {
                        AccessToken = accessToken,
                        ReturnUrl = returnUrl
                    }
                };
                var response = Post<string>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteUserTokenByUserId(string uid, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/SignIn/UserToken/{uid}" : api,
  
                };
                var response = Delete<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<AuthenticateViewModel> FindUserToken(string id, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/SignIn/UserToken/{id}" : api,
                };
                var response = Get<AuthenticateViewModel>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<AuthenticateViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<bool>> SignInAsync(HttpContext httpContext, AuthenticateViewModel model, string authenticationScheme = "", AuthenticationProperties authenticationProperties = null)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                authenticationScheme = (string.IsNullOrEmpty(authenticationScheme)) ? AuthenticationScheme : authenticationScheme;
                
                return BaseResponse<bool>.Success(true);
            }
            catch(Exception e)
            {
                return BaseResponse<bool>.InternalServerError(message: e.Message, fullMsg: e.StackTrace);
            }
        }
        public async Task<BaseResponse<bool>> SignOutAsync(HttpContext httpContext, string authenticationScheme)
        {
            try
            {
                authenticationScheme = (string.IsNullOrEmpty(authenticationScheme)) ? AuthenticationScheme : authenticationScheme;
                await httpContext.SignOutAsync();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.InternalServerError(message: e.Message, fullMsg: e.StackTrace);
            }
        }
        public async Task<BaseResponse<bool>> SingleSignInAsync(HttpContext httpContext, AuthenticateViewModel model, string authenticationScheme = "", AuthenticationProperties authenticationProperties = null)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                authenticationScheme = (string.IsNullOrEmpty(authenticationScheme)) ? SSOAuthenticationScheme : authenticationScheme;
                authenticationProperties = authenticationProperties == null ? new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(365)
                } : authenticationProperties;
                var claims = JwtTokenHelper.GetUserClaims(model);
                var claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);
                var user = new ClaimsPrincipal(claimsIdentity);
                await httpContext.SignInAsync(authenticationScheme, user, authenticationProperties);
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.InternalServerError(message: e.Message, fullMsg: e.StackTrace);
            }
        }
        public async Task<BaseResponse<bool>> SignOutAllDevicesAsync(HttpContext httpContext, string authenticationScheme = "")
        {
            try
            {
                authenticationScheme = (string.IsNullOrEmpty(authenticationScheme)) ? SSOAuthenticationScheme : authenticationScheme;
                await httpContext.SignOutAsync();
                return BaseResponse<bool>.Success(true, authenticationScheme);
            }
            catch (Exception e)
            {
                return BaseResponse<bool>.InternalServerError(message: e.Message, fullMsg: e.StackTrace);
            }
        }

        public BaseResponse<bool> SignOutAllDevicesRequest(string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/SignIn/SignOutAllDevices" : api,
                };
                var response = Get<bool>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<bool> IsAllowedToken(string token, string host = "", string api = "")
        {
            var requestModel = new ApiRequestModel()
            {
                Host = host,
                Api = string.IsNullOrEmpty(api) ? $"api/SignIn/TokenVerification" : api,
                RequestBody = token
            };
            var response = Post<bool>(requestModel);
            return response;
        }

        private static void ThrowIfTokenNullOrEmpty(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token is null or empty.");
        }
    }
}