using APIHelpers.Response;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Identity.Core.Models.Authentications;

namespace Identity.Core.Services.Interfaces
{
    public interface ISignInRequestService : IRequestServiceBase
    {
        BaseResponse<bool> DeleteUserTokenByUserId(string uid, string host = "", string api = "");
        BaseResponse<string> CreateUserToken(string accessToken, string returnUrl, string host = "", string api = "");
        BaseResponse<AuthenticateViewModel> FindUserToken(string id, string host = "", string api = "");
        Task<BaseResponse<bool>> SignInAsync(HttpContext httpContext, AuthenticateViewModel model, string authenticationScheme = "", AuthenticationProperties authenticationProperties = null);
        Task<BaseResponse<bool>> SignOutAsync(HttpContext httpContext, string authenticationScheme);
        Task<BaseResponse<bool>> SingleSignInAsync(HttpContext httpContext, AuthenticateViewModel model, string authenticationScheme = "", AuthenticationProperties authenticationProperties = null);
        Task<BaseResponse<bool>> SignOutAllDevicesAsync(HttpContext httpContext, string authenticationScheme = "");

        BaseResponse<bool> SignOutAllDevicesRequest(string host = "", string api = "");
        BaseResponse<bool> IsAllowedToken(string token, string host = "", string api = "");
    }
}