using APIHelpers.Request;
using AQBooking.Admin.Core.Models.AuthModel;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using AQS.BookingAdmin.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using AQS.BookingAdmin.Services.Interfaces.User;

namespace AQS.BookingAdmin.Services.Implements.User
{
    public class AuthService : BaseService, IAuthService
    {
        #region Fields
        private readonly AdminPortalApiUrl _adminApiUrl;
        private readonly ApiServer _apiServer;      
        public string _identityAdminAPIUrl;
        #endregion

        #region Ctors
        public AuthService(IOptions<AdminPortalApiUrl> adminPortalApiUrlOptions,
            IOptions<ApiServer> apiServerOptions            
            ):base()
        {

            _adminApiUrl = adminPortalApiUrlOptions.Value;
            _apiServer = apiServerOptions.Value;         
            _identityAdminAPIUrl = _apiServer.AQIdentityAdminApi.GetCurrentServer();
        }
        #endregion

        #region Methods

        public async Task<LoginResultModel> AuthorizeUser(LoginModel model)
        {
            try
            {
                string url = _identityAdminAPIUrl + _adminApiUrl.IdentityAdminAPI.Auth;
                var req = new BaseRequest<object>(
                    new
                    {
                        Email = model.UserName,
                        model.Password
                    }
                );
                var res = await _aPIExcute.PostData<LoginResultModel, object>(url:$"{url}", req);

                if (res.IsSuccessStatusCode)
                {
                    return res.ResponseData;
                }
                return new LoginResultModel
                {
                    Message = res.Message ?? "User is not valid"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserInfoModel> GetUser(string token)
        {
            var url = _identityAdminAPIUrl + _adminApiUrl.IdentityAdminAPI.GetAccount;
            var response = await _aPIExcute.GetData<UserInfoModel>(url, token: token);
            if (response.IsSuccessStatusCode)
            {
                return response.ResponseData;
            }
            return null;
        }
        #endregion
    }
}
