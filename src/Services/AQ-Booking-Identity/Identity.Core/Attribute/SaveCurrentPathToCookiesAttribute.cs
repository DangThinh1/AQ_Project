using Identity.Core.Enums;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Identity.Core.Attribute
{
    public class SaveCurrentPathToCookiesAttribute : ActionFilterAttribute
    {
        private string _cookiesName { get; set; }
        private TimeType _timeType { get; set; } = 0;
        private int? _experience { get; set; } = 30;

        public SaveCurrentPathToCookiesAttribute()
        {
            _cookiesName = "AQB_Lst_Path";
        }

        public SaveCurrentPathToCookiesAttribute(string cookiesName)
        {
            _cookiesName = cookiesName;
        }

        public SaveCurrentPathToCookiesAttribute(string cookiesName, TimeType timeType, int? experience)
        {
            _cookiesName = cookiesName;
            _timeType = timeType;
            _experience = experience;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            try
            {
                var request = context.HttpContext.Request;
                if( request !=null)
                {
                    string cookieValueFromContext = context.HttpContext.Request.Cookies[_cookiesName];
                    // Only save last path with method GET, don't save another method
                    if (request.Method == "GET")
                        cookieValueFromContext = WebUtility.UrlEncode(string.Format("{0}{1}{2}",request.PathBase,request.Path,request.QueryString.Value)); 
                    // write to cookies
                    CookiesHelper.SetCookies(context.HttpContext, _timeType, _cookiesName, cookieValueFromContext, _experience);
                }
            }
            catch
            {
                return;
            }
        }


    }
}
