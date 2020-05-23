using System;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.CommonValues;
using AQConfigurations.Core.Services.Interfaces;

namespace AQConfigurations.Core.Services.Implements
{
    public class CommonValueRequestService : ConfigurationsRequestServiceBase, ICommonValueRequestService
    {
        private const string FIND_URL = "api/CommonValues/{0}/Lang/{1}";
        private const string GET_ALL_URL = "api/CommonValues/Lang/{0}";
        private const string GET_BY_GROUP_URL = "api/CommonValues/ValueGroup/{0}/Lang/{1}";
        private const string GET_BY_GROUP_INT_URL = "api/CommonValues/ValueGroup/{0}/ValueInt/{1}/Lang/{2}";
        private const string GET_BY_GROUP_DOUBLE_URL = "api/CommonValues/ValueGroup/{0}/ValueDouble/{1}/Lang/{2}";
        private const string GET_BY_GROUP_STRING_URL = "api/CommonValues/ValueGroup/{0}/ValueString/{1}/Lang/{2}";
        public CommonValueRequestService() : base()
        {
        }

        public BaseResponse<CommonValueViewModel> Find(int id, int lang = 1, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(FIND_URL, id, lang);

                string url = _configurationsHost + actionUrl;

                var response = Get<CommonValueViewModel>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CommonValueViewModel>> GetAllCommonValue(int lang = 1, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(GET_ALL_URL, lang);

                string url = _configurationsHost + actionUrl;

                var response = Get<List<CommonValueViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommonValueViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CommonValueViewModel>> GetListCommonValueByGroup(string valueGroup, int lang = 1, string apiUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiUrl))
                    apiUrl = string.Format(GET_BY_GROUP_URL, valueGroup, lang);

                string url = _configurationsHost + apiUrl;

                var response = Get<List<CommonValueViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CommonValueViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CommonValueViewModel> GetCommonValueByGroupInt(string valueGroup, int valueInt, int lang = 1, string apiUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiUrl))
                    apiUrl = string.Format(GET_BY_GROUP_INT_URL, valueGroup, valueInt, lang);

                string url = _configurationsHost + apiUrl;

                var response = Get<CommonValueViewModel>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CommonValueViewModel> GetCommonValueByGroupString(string valueGroup, string valueString, int lang = 1, string apiUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiUrl))
                    apiUrl = string.Format(GET_BY_GROUP_STRING_URL, valueGroup, valueString, lang);

                string url = _configurationsHost + apiUrl;

                var response = Get<CommonValueViewModel>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CommonValueViewModel> GetCommonValueByGroupDouble(string valueGroup, double valueDouble, int lang = 1, string apiUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiUrl))
                    apiUrl = string.Format(GET_BY_GROUP_DOUBLE_URL, valueGroup, valueDouble, lang);

                string url = _configurationsHost + apiUrl;

                var response = Get<CommonValueViewModel>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<CommonValueViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
