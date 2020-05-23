using APIHelpers;
using APIHelpers.Request;
using AQBooking.Admin.Core.Models.CommonValue;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Interfaces.User;
using AQS.BookingAdmin.Models.Paging;
using AQS.BookingAdmin.Services.Interfaces.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Implements.Common
{
    public class CommonValueService : BaseService, ICommonValueService
    {
        #region Fields
        private readonly AdminPortalApiUrl _AdminApiUrl;
        private readonly string _baseAQAdminAPIUrl;
        private readonly string commonValueUrl;
        private readonly string commonValueSearchUrl;
        private readonly string commonValueCustomUrl;
        private readonly string commonValuesLstURL;
        #endregion

        #region Ctor
        public CommonValueService(IOptions<AdminPortalApiUrl> adminPortalAP, IOptions<ApiServer> _apiServer) : base()
        {
            _AdminApiUrl = adminPortalAP.Value;
            _baseAQAdminAPIUrl = _apiServer.Value.AQAdminApi.GetCurrentServer();
            commonValueUrl = _baseAQAdminAPIUrl + _AdminApiUrl.CommonValueAPI.Index;
            commonValuesLstURL = _baseAQAdminAPIUrl + _AdminApiUrl.CommonValueAPI.CommonValues;
            commonValueSearchUrl = _baseAQAdminAPIUrl + _AdminApiUrl.CommonValueAPI.SearchCommonValue;
            commonValueCustomUrl = _baseAQAdminAPIUrl + _AdminApiUrl.CommonValueAPI.GetValueGroupLst;
        }
        #endregion

        #region Methods
        public async Task<CommonValueViewModel> GetById(int Id)
        {
            var apiResult = await _aPIExcute.GetData<object>($"{commonValueUrl}/{Id}", null, Token);
            var res = JsonConvert.DeserializeObject<CommonValueViewModel>(JsonConvert.SerializeObject(apiResult.ResponseData));
            return res;
        }

        public async Task<PagedListClientModel<CommonValueViewModel>> GetAllCommonValuesPaging(CommonValueSearchModel model)
        {
            try
            {
                Dictionary<string, object> requestParam = new Dictionary<string, object>();
                requestParam.Add("ValueGroup", model.ValueGroup);
                requestParam.Add("ValueString", model.ValueString);
                requestParam.Add("Text", model.Text);
                requestParam.Add("PageIndex", model.PageIndex);
                requestParam.Add("PageSize", model.PageSize);
                requestParam.Add("SortColumn", model.SortColumn);
                requestParam.Add("SortString", model.SortString);
                requestParam.Add("SortType", model.SortType);

                var apiResult = await _aPIExcute.GetData<PagedListClientModel<CommonValueViewModel>>(commonValueSearchUrl, requestParam, Token);

                if (apiResult.IsSuccessStatusCode)
                    return apiResult.ResponseData;

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CommonValueViewModel>> GetAllCommonValues()
        {
            var apiResult = await _aPIExcute.GetData<object>(commonValuesLstURL, null, Token);
            var res = JsonConvert.DeserializeObject<List<CommonValueViewModel>>(JsonConvert.SerializeObject(apiResult.ResponseData));
            return res;
        }
        public async Task<List<CommonValueViewModel>> GetCommonValuesByGroupName(string groupName)
        {
            var apiResult = await _aPIExcute.GetData<List<CommonValueViewModel>>($"{_AdminApiUrl.CommonValueAPI.GetByGroupName}/{groupName}", null, Token);
            return apiResult.GetDataResponse();
        }
        public async Task<List<string>> GetValueGroupStr()
        {
            var apiResult = await _aPIExcute.GetData<List<string>>(commonValueCustomUrl, null, Token);
            var res = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(apiResult.ResponseData));
            return res;
        }
        public async Task<object> CreateNewCommonValues(CommonValueCreateModel model)
        {
            try
            {
                var req = new BaseRequest<CommonValueCreateModel>(model);
                var apiResult = await _aPIExcute.PostData<object, CommonValueCreateModel>(commonValueUrl, HttpMethodEnum.POST, req, Token);
                return apiResult.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public async Task<object> UpdateCommonValues(CommonValueUpdateModel model)
        {
            try
            {
                var req = new BaseRequest<CommonValueUpdateModel>(model);
                var apiResult = await _aPIExcute.PostData<object, CommonValueUpdateModel>(commonValueUrl, HttpMethodEnum.PUT, req, Token);
                return apiResult.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
        public Task<object> DeleteCommonValues(int Id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
