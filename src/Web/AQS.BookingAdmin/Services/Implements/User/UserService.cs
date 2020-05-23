using APIHelpers.Response;
using AQBooking.Admin.Core.Models.AuthModel;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Interfaces.User;
using AQS.BookingAdmin.Services.Interfaces.Common;
using AQS.BookingAdmin.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.User
{
    public class UserService:BaseService,IUserService
    {

        #region Fields
        private const string ALL_USER_CACHE = "ALL_USER_CACHE";
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public UserService(IWorkContext workContext,
            ICacheManager cacheManager
            ) :base()
        {
            _workContext = workContext;
            _cacheManager = cacheManager;
            
        }
        #endregion
        #region Methods
        public async Task<BaseResponse<List<UserInfoModel>>> GetAllUser()
        {
            try
            {
                return await _cacheManager.GetAsync(ALL_USER_CACHE, async () =>
                {
                    string urlRequest = $"{_apiServer.AQIdentityAdminApi.GetCurrentServer()}{_adminPortalApiUrl.IdentityAdminAPI.GetAllAccounts}";
                    var res = await _aPIExcute.GetData<List<UserInfoModel>>(urlRequest, null, _workContext.UserToken);
                    return res;
                });
               
            }
            catch (Exception ex)
            {
                return BaseResponse<List<UserInfoModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion

    }
}
