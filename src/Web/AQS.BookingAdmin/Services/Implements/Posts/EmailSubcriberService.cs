using APIHelpers.Request;
using APIHelpers.Response;
using AQBooking.Admin.Core.Models.Subscriber;
using AQS.BookingAdmin.Infrastructure.AQPagination;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.Posts
{
    public class EmailSubcriberService : BaseService, IEmailSubcriberService
    {



        #region Ctor
        public EmailSubcriberService()
        {

        }
        #endregion

        #region Methods
        public async Task<BaseResponse<PagedListClient<SubscriberViewModel>>> SearchEmailSubcriber(SubscriberSearchModel searchModel)
        {
            try
            {
                var requestParams = ConvertToUrlParameter(searchModel);
                string urlRequest = $"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.SearchEmailSubcriber}{requestParams}";
                var res = await _aPIExcute.GetData<PagedListClient<SubscriberViewModel>>(urlRequest, null, Token);
                return res;
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedListClient<SubscriberViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public async Task<List<SubscriberViewModel>> GetLstSubcriToExport(SubscriberSearchModel searchModel)
        {
            var req = new BaseRequest<SubscriberSearchModel>(searchModel);
            var res = await _aPIExcute.PostData<List<SubscriberViewModel>, SubscriberSearchModel>($"{_baseAdminAPIUrl}{_adminPortalApiUrl.PostAPI.GetListSubToExport}", APIHelpers.HttpMethodEnum.POST, req, Token);
            if (!res.IsSuccessStatusCode)
                return null;
            return res.ResponseData;
        }
        #endregion


    }
}
