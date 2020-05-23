using APIHelpers.Response;
using Identity.Core.Models.Authentications;
namespace Identity.Infrastructure.Services.Interfaces
{
    public interface IAuthenticateService
    {
        BaseResponse<AuthenticateViewModel> AuthenticateByToken(string accessToken);
        BaseResponse<AuthenticateViewModel> AuthenticateByEmail(string email, string password);
        BaseResponse<AuthenticateViewModel> AuthenticateByFaceBook(string facebookAccessToken, string userId);
         BaseResponse<AuthenticateViewModel> AuthenticateByGoogle(GoogleAuthenticateModel googleAuthenticateModel);
    }
}
