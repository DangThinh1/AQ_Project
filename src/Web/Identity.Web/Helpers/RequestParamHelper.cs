
using Identity.Core.Helpers;
using System;
using Microsoft.AspNetCore.Http;

namespace Identity.Web.Helpers
{
    public class RequestParamHelper
    {
        private const string DEFAULT_RETURN_URL_PARAMETER_NAME = "returnUrl";//the default param name of return url in the request url
        private static readonly IHttpContextAccessor _httpContextAccessor = IdentityInjectionHelper.GetService<IHttpContextAccessor>()
                                                                   ?? throw new ArgumentNullException("IHttpContextAccessor is null");
        private static HttpContext _httpContext => _httpContextAccessor.HttpContext;

        public static string GetReturUrlFromQuery(string returnUrlKey = DEFAULT_RETURN_URL_PARAMETER_NAME)
        {
            try
            {
                string returlUrl = _httpContext.Request.Query[returnUrlKey].ToString();
                return returlUrl ?? string.Empty;
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetReturUrlFromCookie(string returnUrlKey = DEFAULT_RETURN_URL_PARAMETER_NAME)
        {
            try
            {
                string returlUrl = _httpContext.Request.Cookies[returnUrlKey].ToString();
                return returlUrl ?? string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}