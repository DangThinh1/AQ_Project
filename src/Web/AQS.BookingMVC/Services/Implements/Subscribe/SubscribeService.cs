using APIHelpers;
using APIHelpers.Request;
using APIHelpers.Response;
using AQBooking.Admin.Core.Models.Subscriber;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Models.Config;
using AQS.BookingMVC.Services.Interfaces.Subscribe;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Subscribe
{
    public class SubscribeService : ServiceBase, ISubscribeService
    {
        #region Fields
        private readonly AdminApiUrl _adminApiUrl;
        private string _baseAdminApiUrl = ApiUrlHelper.AdminApiUrl;
        #endregion
        public SubscribeService(IOptions<AdminApiUrl> adminApiUrlOption) : base()
        {
            _adminApiUrl = adminApiUrlOption.Value;
        }
        public async Task<BaseResponse<bool>> Subscribe(SubscriberCreateModel createModel)
        {
            try
            {
                var apiExecute = new APIExcute(AuthenticationType.Bearer);

                var url = string.Format("{0}{1}",
                    _baseAdminApiUrl,
                    _adminApiUrl.Post.Subscribe);

                var response = await apiExecute.PostData<bool, SubscriberCreateModel>(
                    url,
                    HttpMethodEnum.POST,
                    new BaseRequest<SubscriberCreateModel>(createModel),
                    basicToken);

                return response;
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }
    }
}
