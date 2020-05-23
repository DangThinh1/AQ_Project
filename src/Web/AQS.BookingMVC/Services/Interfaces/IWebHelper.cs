using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces
{
    public interface IWebHelper
    {
        #region Host Name
        /// <summary>
        /// Get current host name
        /// </summary>      
        /// <returns>host name http(s)/{hostname}:{port}/</returns>
        string GetHostName();
        #endregion

        #region Url 
        /// <summary>
        /// Get current request page url
        /// </summary>
        /// <param name="includeQueryString">include query string</param>
        ///  /// <param name="useSsl">use ssl or not default auto detect from host</param>
        /// <param name="lowercaseUrl">lower case url</param>
        /// <returns>full url</returns>
        string GetThisPageUrl(bool includeQueryString, bool? useSsl = null,bool lowercaseUrl = false,bool includeLanguageParams=false);
        string GetQueryString(bool includeNullValue = false);
        #endregion
    }
}
