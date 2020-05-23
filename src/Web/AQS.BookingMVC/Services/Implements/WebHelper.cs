using AQS.BookingMVC.Infrastructure.Constants;
using AQS.BookingMVC.Interfaces;
using AQS.BookingMVC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements
{
    public class WebHelper:IWebHelper
    {

        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor
        public WebHelper(IHttpContextAccessor httpContextAccessor,
            IWorkContext workContext
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _workContext = workContext;
        }
        #endregion

        #region Methods
        #region Host 
        
        public  string GetHostName()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //try to get host from the request HOST header
            var hostHeader = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
                return string.Empty;
            bool useSsl = IsCurrentConnectionSecured();
            //add scheme to the URL
            var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

            //ensure that host is ended with slash
            storeHost = $"{storeHost.TrimEnd('/')}/";

            return storeHost;
        }
        #endregion
        #region Url
        public string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false, bool includeLanguageParams = false)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            //get  location
            var location = GetHostName();

            //add local path to the URL
            var pageUrl = $"{location.TrimEnd('/')}{_httpContextAccessor.HttpContext.Request.Path}";

            //add query string to the URL
            if (includeQueryString)
                pageUrl = $"{pageUrl}{_httpContextAccessor.HttpContext.Request.QueryString}";

            //whether to convert the URL to lower case
            if (lowercaseUrl)
                pageUrl = pageUrl.ToLowerInvariant();
            if(includeLanguageParams)
            {
                if (pageUrl.Contains("?"))
                    pageUrl += "&";
                else
                    pageUrl += "?";

                pageUrl += $"{CommonValueConstant.LANGUAGE_QUERY_STRING_NAME}={_workContext.CurrentLanguageId}";
            }
            return pageUrl;
        }
        public string GetQueryString(bool includeNullValue=false)
        {
            string queryString = _httpContextAccessor.HttpContext.Request.QueryString.ToString();
            if (includeNullValue)
                return queryString;
        
            var queryParameters = QueryHelpers.ParseQuery(queryString);
            string query = "";
            foreach(var q in queryParameters)
            {
                if (!string.IsNullOrEmpty(q.Value)&&q.Value!="0")
                    query += $"{q.Key}={q.Value}&";
            }
            if (query != "")
                return $"?{query.TrimEnd('&')}";
            return query;
        }
        #endregion
        #region Utilities
        protected bool IsRequestAvailable()
        {
            if (_httpContextAccessor?.HttpContext == null)
                return false;

            try
            {
                if (_httpContextAccessor.HttpContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public bool IsCurrentConnectionSecured()
        {
            return _httpContextAccessor.HttpContext.Request.Scheme== Uri.UriSchemeHttps;
       
        }
        #endregion
        #endregion

    }
}
