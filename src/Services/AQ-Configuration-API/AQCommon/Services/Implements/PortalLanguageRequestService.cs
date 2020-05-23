using System;
using System.Linq;
using APIHelpers.Response;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Models.PortalLanguages;

namespace AQConfigurations.Core.Services.Implements
{
    public class PortalLanguageRequestService : ConfigurationsRequestServiceBase, IPortalLanguageRequestService
    {
        private const string GET_LIST_LANGUAGES_BY_PORTAL_UID_URL = "api/PortalLanguages/{0}";
        private const string GET_LIST_LANGUAGES_BY_DOMAIN_ID_URL = "api/PortalLanguages/DomainId/{0}";
        public PortalLanguageRequestService() : base()
        {
        }
        public BaseResponse<List<SelectListItem>> GetLanguagesAsSelectListByDomainId(int domainId, string actionUrl = "")
        {
            try
            {
                var languagesResponse = GetLanguagesByDomainId(domainId, actionUrl);
                if(languagesResponse.IsSuccessStatusCode)
                {
                    var data = languagesResponse.ResponseData;
                    var selectList = data.Select(k => new SelectListItem(k.LanguageName, k.LanguageFid.ToString())).ToList();
                    return BaseResponse<List<SelectListItem>>.Success(selectList);
                }
                return BaseResponse<List<SelectListItem>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SelectListItem>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<List<SelectListItem>> GetLanguagesAsSelectListByDomainUID(string portalUniqueId, string actionUrl = "")
        {
            try
            {
                var languagesResponse = GetLanguagesByPortalUID(portalUniqueId, actionUrl);
                if (languagesResponse.IsSuccessStatusCode)
                {
                    var data = languagesResponse.ResponseData;
                    var selectList = data.Select(k => new SelectListItem(k.LanguageName, k.LanguageFid.ToString())).ToList();
                    return BaseResponse<List<SelectListItem>>.Success(selectList);
                }
                return BaseResponse<List<SelectListItem>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SelectListItem>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<PortalLanguageViewModel>> GetLanguagesByDomainId(int domainId, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(GET_LIST_LANGUAGES_BY_DOMAIN_ID_URL, domainId);

                string url = _configurationsHost + actionUrl;

                var response = Get<List<PortalLanguageViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLanguageViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<List<PortalLanguageViewModel>> GetLanguagesByPortalUID(string portalUniqueId, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(GET_LIST_LANGUAGES_BY_PORTAL_UID_URL, portalUniqueId);

                string url = _configurationsHost + actionUrl;

                var response = Get<List<PortalLanguageViewModel>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLanguageViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}