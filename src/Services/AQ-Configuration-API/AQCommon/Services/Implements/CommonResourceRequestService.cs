using System;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Models.CommonResources;

namespace AQConfigurations.Core.Services.Implements
{
    public class CommonResourceRequestService : ConfigurationsRequestServiceBase, ICommonResourceRequestService
    {
        private const string ALL_RESOURCE = "api/v1.0​/ConfigurationAPI/CommonResources/{0}";
        private const string GET_BY_RESOURCE_KEY = "api/v1.0​/ConfigurationAPI/CommonResources/ResourceValue/{0}";
        public CommonResourceRequestService() : base()
        {
        }
        public BaseResponse<List<CommonResourceViewModel>> GetAllResource(int languageID, List<string> type, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(ALL_RESOURCE, languageID);
                string url = _configurationsHost + actionUrl;
                var response = Post<List<CommonResourceViewModel>>(url, type);
                return response;
            }
            catch(Exception ex)
            {
                return BaseResponse<List<CommonResourceViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<string> GetResourceValue(int lang, string resourceKey, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(GET_BY_RESOURCE_KEY, lang);
                string url = _configurationsHost + actionUrl;
                var response = Post<string>(url, resourceKey);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
} 