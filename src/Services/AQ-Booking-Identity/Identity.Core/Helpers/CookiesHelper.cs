using Identity.Core.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Identity.Core.Helpers
{
    public static class CookiesHelper
    {
        
        /// <summary>
        /// Set the cookies
        /// </summary>
        /// <param name="context">the current context</param> 
        /// <param name="key">key (unique indentifier) cookies name</param>
        /// <param name="value">value to store in cookie object</param>
        /// <param name="timeType">type of time expire {0: day; 1: hour; 2:minute ; 3: second ; default :0 ; else input> 3 --> default:minutes } </param>
        /// <param name="expireTime">expiration time</param>
        public static void SetCookies(this HttpContext context, TimeType timeType = 0 ,string key =null, string value =null, int? expireTime = 30)
        {
            CookieOptions option = new CookieOptions();
           
            switch ((int)timeType)
            {
                case 0:
                    if (expireTime.HasValue)
                        option.Expires = DateTime.Now.AddDays(expireTime.Value);
                    else
                        option.Expires = DateTime.Now.AddDays(1);

                    context.Response.Cookies.Append(key, value, option);
                    break;

                case 1:
                    if (expireTime.HasValue)
                        option.Expires = DateTime.Now.AddHours(expireTime.Value);
                    else
                        option.Expires = DateTime.Now.AddHours(1);
                    context.Response.Cookies.Append(key, value, option);
                    break;

                case 2:
                    if (expireTime.HasValue)
                        option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                    else
                        option.Expires = DateTime.Now.AddMinutes(1);
                    context.Response.Cookies.Append(key, value, option);
                    break;
                case 3:
                    if (expireTime.HasValue)
                        option.Expires = DateTime.Now.AddSeconds(expireTime.Value);
                    else
                        option.Expires = DateTime.Now.AddSeconds(1);
                    context.Response.Cookies.Append(key, value, option);
                    break;
                default:
                    if (expireTime.HasValue)
                        option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                    else
                        option.Expires = DateTime.Now.AddMinutes(10);

                    context.Response.Cookies.Append(key, value, option);
                    break;

            }
            
        }

        /// <summary>  
        /// Delete the cookies key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public static void RemoveCookies(this HttpContext context, string key)
        {
            context.Response.Cookies.Delete(key);
        }


        public static string GetCookies(this HttpContext context, string key)
        {
            var cookieValueFromContext = string.Empty;
            cookieValueFromContext= context.Request.Cookies[key];

            return cookieValueFromContext;
        }




    }
}
