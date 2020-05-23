using System;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.CommonValues;
using AQBooking.YachtPortal.Web.Interfaces.Common;
using AQS.BookingMVC.Models.Config;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.Common
{
    public class CommonValueService : ServiceBase, ICommonValueService
    {
        private readonly BaseApiUrl _baseApiUrl;
        public CommonValueService(IOptions<BaseApiUrl> _baseApiUrlOptions)
        {
            _baseApiUrl = _baseApiUrlOptions.Value;
        }
        public async Task<BaseResponse<List<CommonValueViewModel>>> GetAllCommonValue()
        {
            try
            {                
                string url = _baseConfigurationApi + _baseApiUrl.CommonValues.GetAllCommonValue;
                var response = await _apiExcute.GetData<List<CommonValueViewModel>>(url, null);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommonValueViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<CommonValueViewModel>> GetCommonValueByGroupDouble(string valueGroup, double valueDouble)
        {
            try
            {
               
                string url =string.Format($"{_baseConfigurationApi}{_baseApiUrl.CommonValues.GetCommonValueByValueDouble}",valueGroup,valueDouble);
                var response =await _apiExcute.GetData<CommonValueViewModel>(url, null);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<CommonValueViewModel>> GetCommonValueByGroupInt(string valueGroup, int valueInt)
        {
            try
            {

                string url = string.Format($"{_baseConfigurationApi}{_baseApiUrl.CommonValues.GetCommonValueByGroupInt}", valueGroup, valueInt);
                var response = await _apiExcute.GetData<CommonValueViewModel>(url, null);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<CommonValueViewModel>> GetCommonValueByGroupString(string valueGroup, string valueString)
        {
            try
            {

                string url = string.Format($"{_baseConfigurationApi}{_baseApiUrl.CommonValues.GetCommonValueByValueString}", valueGroup, valueString);
                var response = await _apiExcute.GetData<CommonValueViewModel>(url, null);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<BaseResponse<List<CommonValueViewModel>>> GetListCommonValueByGroup(string valueGroup)
        {
            try
            {

                string url = string.Format($"{_baseConfigurationApi}{_baseApiUrl.CommonValues.GetListCommonValueByGroup}", valueGroup);
                var response = await _apiExcute.GetData<List<CommonValueViewModel>>(url, null);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommonValueViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}