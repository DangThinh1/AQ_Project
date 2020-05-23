using APIHelpers.Response;
using Identity.Core.Services.Interfaces;
using Identity.Core.Models.Authentications;

namespace Identity.Core.Services.Implements
{
    public class AuthenticateRequestService : RequestServiceBase, IAuthenticateRequestService
    {
        public AuthenticateRequestService(IUsersContext usersContext) : base(usersContext)
        {
        }

        public AuthenticateRequestService(string a, IUsersContext usersContext) : base(usersContext)
        {
        }

        public BaseResponse<AuthenticateViewModel> Authenticate(string email, string password, string host = "", string api = "", string token="")
        {
            var requestModel = new ApiRequestModel() {
                Token = token,
                Host = host,
                Api = string.IsNullOrEmpty(api) ? "api/Auth" : api,
                RequestBody = new AuthModel() {
                    Email = email,
                    Password = password
                }
            };
            var response = Post<AuthenticateViewModel>(requestModel);
            return response;
        }

        public BaseResponse<AuthenticateViewModel> FacebookAuthenticate(string userId, string accessToken, string host = "", string api = "", string token = "")
        {
            var requestModel = new ApiRequestModel()
            {
                Token = token,
                Host = host,
                Api = string.IsNullOrEmpty(api) ? "api/FacebookAuth" : api,
                RequestBody = new FacebookAuthModel()
                {
                    UserId = userId,
                    AccessToken = accessToken
                }
            };
            var response = Post<AuthenticateViewModel>(requestModel);
            return response;
        }
        public BaseResponse<AuthenticateViewModel> GoogleAuthenticate(GoogleAuthenticateModel googleAuthenticateModel, string host = "", string api = "", string token = "")
        {
            var requestModel = new ApiRequestModel()
            {
                Token = token,
                Host = host,
                Api = string.IsNullOrEmpty(api) ? "api/GoogleAuth" : api,
                RequestBody = googleAuthenticateModel
            };
            var response = Post<AuthenticateViewModel>(requestModel);
            return response;
        }
    }
}
