using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Identity.Core.Helpers
{
    public class CookieManager : ICookieManager
    {
        #region Private Members

        private readonly ICookieManager _concreteManager;

        #endregion


        #region Private Methods

        private string RemoveSubdomain(string host)
        {
            var splitHostname = host.Split('.');
            //if not localhost
            if (splitHostname.Length > 1)
            {
                return string.Join(".", splitHostname.Skip(1));
            }
            else
            {
                return host;
            }
        }

        #endregion

        #region Public Methods

        public CookieManager()
        {
            _concreteManager = new ChunkingCookieManager();
        }

        public void AppendResponseCookie(HttpContext context, string key, string value, CookieOptions options)
        {
            options.Domain = RemoveSubdomain(context.Request.Host.Host);  //Set the Cookie Domain using the request from host
            _concreteManager.AppendResponseCookie(context, key, value, options);
        }

        public void DeleteCookie(HttpContext context, string key, CookieOptions options)
        {
            _concreteManager.DeleteCookie(context, key, options);
        }

        public string GetRequestCookie(HttpContext context, string key)
        {
            return _concreteManager.GetRequestCookie(context, key);
        }

        #endregion
    }
}
