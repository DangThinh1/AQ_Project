using APIHelpers.Response;
using Identity.Core.Models.Authentications;

namespace Identity.Infrastructure.Services.Interfaces
{
    public interface IUserTokenService
    {
        BaseResponse<AuthenticateViewModel> FindUserToken(string id);
        BaseResponse<string> Create(string token, string returnUrl);
        BaseResponse<bool> DeleteUserTokenByUserId(string uid);
    }
}