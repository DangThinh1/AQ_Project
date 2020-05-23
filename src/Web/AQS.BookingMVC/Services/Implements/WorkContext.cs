using AQConfigurations.Core.Models.CommonLanguages;
using AQConfigurations.Core.Services.Interfaces;
using AQS.BookingMVC.Infrastructure.Constants;
using AQS.BookingMVC.Interfaces;
using AQS.BookingMVC.Models.Config;
using Identity.Core.Helpers;
using Identity.Core.Models.Authentications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services
{
    public class WorkContext : IWorkContext
    {
        #region Fields
        private AuthenticateViewModel _currentUser = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonLanguagesRequestServices _commonLanguagesRequestServices;
        private readonly IMultiLanguageService _multiLanguageService;
        private readonly Setting _setting;
        private string _currentLanguage;
        private int _currentLanguageId;
        private List<CommonLanguagesViewModel> _listLanguageCommon;
        #endregion

        #region Ctor
        public WorkContext(IHttpContextAccessor httpContextAccessor,
            IOptions<Setting> setting,
            ICommonLanguagesRequestServices commonLanguagesRequestServices,
            IMultiLanguageService multiLanguageService
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _setting = setting.Value;
            _commonLanguagesRequestServices = commonLanguagesRequestServices;
            _multiLanguageService = multiLanguageService;

        }
        #endregion

        #region Property
        public AuthenticateViewModel CurentUser => GetCurrentUser();

        public bool IsAuthentication => CurentUser != null;

        public bool IsComingSoon => _setting.IsComingSoon;

        public List<CommonLanguagesViewModel> ListLanguageCommon
        {
            get
            {
                if (_listLanguageCommon != null)
                    return _listLanguageCommon;

                var response = _commonLanguagesRequestServices.GetAllLanguages();
                if (response.IsSuccessStatusCode
                    && (response.ResponseData != null))
                {
                    _listLanguageCommon = response.ResponseData;
                }
                if (_listLanguageCommon == null)
                    _listLanguageCommon = new List<CommonLanguagesViewModel>();
                return _listLanguageCommon;
            }
        }

        //public string CurrentLanguageCode => (_httpContextAccessor.HttpContext.Request.Cookies["langCode"] != null)
        //    ? _httpContextAccessor.HttpContext.Request.Cookies["langCode"].ToString().ToLower()
        //    : "en-us";
        public string CurrentLanguageCode
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                {
                    return _currentLanguage;
                }
                return (_currentLanguage = ListLanguageCommon.FirstOrDefault(x => x.Id == CurrentLanguageId)?.LanguageCode);
               
            }
        }
        public int CurrentLanguageId
        {
            get
            {
                if (_currentLanguageId > 0)
                    return _currentLanguageId;
                _currentLanguageId = _multiLanguageService.CurrentLaguageId;
                //if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("lang"))
                //{
                //    try
                //    {
                //        string cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["lang"]?.ToString();
                //        if (cookieValue != null)
                //            _currentLanguageId = Convert.ToInt32(cookieValue);
                //    }
                //    catch
                //    {

                //    }
                //}
                //if (_currentLanguageId==0&&_httpContextAccessor.HttpContext.Request.Query.ContainsKey(CommonValueConstant.LANGUAGE_QUERY_STRING_NAME))
                //{

                //    try
                //    {
                //        var queryValue = _httpContextAccessor.HttpContext.Request.Query[CommonValueConstant.LANGUAGE_QUERY_STRING_NAME];
                //        if (queryValue.ToString()!="")
                //            _currentLanguageId = Convert.ToInt32(queryValue);
                //    }
                //    catch
                //    {

                //    }
                //}
                //if (_currentLanguageId == 0)
                //    _currentLanguageId = CommonValueConstant.DEFAULT_LANGUAGE_ID;//default english

                return _currentLanguageId;
            }
        }
        #endregion

        #region Method
        private AuthenticateViewModel GetCurrentUser()
        {
            if (_currentUser != null)
                return _currentUser;
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return null;
            _currentUser = JwtTokenHelper.GetSignInProfile(_httpContextAccessor.HttpContext.User);
            return _currentUser;
        }
        #endregion
    }
}
