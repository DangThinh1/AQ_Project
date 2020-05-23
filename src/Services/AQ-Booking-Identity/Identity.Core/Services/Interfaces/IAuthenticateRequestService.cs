using APIHelpers.Response;
using Identity.Core.Models.Authentications;

namespace Identity.Core.Services.Interfaces
{
    public interface IAuthenticateRequestService : IRequestServiceBase
    {
        //For AQ Account
        BaseResponse<AuthenticateViewModel> Authenticate(string email, string password, string host = "", string api = "", string token="");

        //For EndUser
        BaseResponse<AuthenticateViewModel> FacebookAuthenticate(string userId, string accessToken, string host = "", string api = "", string token ="");

        BaseResponse<AuthenticateViewModel> GoogleAuthenticate(GoogleAuthenticateModel googleAuthenticateModel, string host = "", string api = "", string token = "");
    }
}
