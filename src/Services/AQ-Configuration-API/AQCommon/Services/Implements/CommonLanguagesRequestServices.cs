using System;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Models.CommonLanguages;

namespace AQConfigurations.Core.Services.Implements
{
    public class CommonLanguagesRequestServices : ConfigurationsRequestServiceBase, ICommonLanguagesRequestServices
    {
        private const string GET_ALL_URL = "api/v1.0/ConfigurationAPI/CommonLanguagues";
        public CommonLanguagesRequestServices() : base()
        {
        }

        public BaseResponse<List<CommonLanguagesViewModel>> GetAllCommonValue(string actionUrl = "") => GetAllLanguages(actionUrl);

        public BaseResponse<List<CommonLanguagesViewModel>> GetAllLanguages(string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = GET_ALL_URL;

                string url = _configurationsHost + actionUrl;

                var response = Get<List<CommonLanguagesViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommonLanguagesViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}