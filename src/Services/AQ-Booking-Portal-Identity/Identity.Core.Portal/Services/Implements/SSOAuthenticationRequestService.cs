using APIHelpers.Response;
using AQEncrypts;
using Identity.Core.Helpers;
using Identity.Core.Models.Authentications;
using Identity.Core.Portal.Models.SSOAuthentication;
using Identity.Core.Portal.Services.Interfaces;
using Identity.Core.Services.Base;
using System;
using System.Collections.Generic;

namespace Identity.Core.Portal.Services.Implements
{
    public class SSOAuthenticationRequestService : RequestServiceBase, ISSOAuthenticationRequestService
    {
        private const string CREATE_URL = "api/SSOAuthentication";
        private const string FIND_BY_ID_URL = "api/SSOAuthentication/Id/";
        private const string FIND_BY_DOMIAN_ID_URL = "api/SSOAuthentication/Id/{0}/DomainId/{1}";
        private const string DELETE_BY_ID_URL = "api/SSOAuthentication/Id/";
        private const string CLEAR_URL = "api/SSOAuthentication/UserUid/";
        private const string GET_PROFILE_URL = "api/SSOAuthentication/Profile/AuthId/{0}/DomainId/{1}";
        private string _host = IdentityInjectionHelper.GetBaseUrl();

        public BaseResponse<string> Create(List<SSOAuthenticationCreateModel> createModel) 
        {
            try
            {
                var url = _host + CREATE_URL;
                var baseResponse = Post<string>(url, createModel);
                return baseResponse;
            }
            catch(Exception ex)
            {
                return BaseResponse<string>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<AuthenticateViewModel> GetProfile(string authId, string domainId)
        {
            try
            {
                var authIdEncrypted = authId.Encrypt();
                var url = _host + string.Format(GET_PROFILE_URL, authIdEncrypted, domainId);
                var baseResponse = Get<AuthenticateViewModel>(url);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return BaseResponse<AuthenticateViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<SSOAuthenticationViewModel>> FindById(string id)
        {
            try
            {
                var url = _host + CREATE_URL + id;
                var baseResponse = Get<List<SSOAuthenticationViewModel>>(url);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SSOAuthenticationViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<SSOAuthenticationViewModel> FindByDomainId(string id, string domainId)
        {
            try
            {
                var url = _host + string.Format(FIND_BY_DOMIAN_ID_URL, id, domainId);
                var baseResponse = Get<SSOAuthenticationViewModel>(url);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return BaseResponse<SSOAuthenticationViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> DeleteById(string id)
        {
            try
            {
                var url = _host + DELETE_BY_ID_URL + id;
                var baseResponse = Delete<bool>(url);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> Clear(string uid)
        {
            try
            {
                var url = _host + CLEAR_URL + uid;
                var baseResponse = Delete<bool>(url);
                return baseResponse;
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
