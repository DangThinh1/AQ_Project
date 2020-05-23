using AQS.BookingMVC.Infrastructure.Constants;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.Mvc.CustomMiddleware
{
    public class LanguageMidleware
    {

        private readonly RequestDelegate _next;
        private readonly IActionContextAccessor _actionContextAccessor;
      

        public LanguageMidleware(RequestDelegate next, IActionContextAccessor actionContextAccessor
            )
        {
            _actionContextAccessor = actionContextAccessor;
        
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
           
            // code executed before the next middleware
            await _next.Invoke(context); // call next middleware

            // code executed after the next middleware
        }
    }
    public static class LanguageMidlewareExtensions
    {
        public static IApplicationBuilder UseLanguageMiddleware(this IApplicationBuilder app)
        {
            //app.Use((context,next)=> {
            //    try
            //    {
            //        var route = context.GetRouteData();
            //        var langCode = route.Values["lang"];
            //        if (langCode != null)
            //        {
            //            if (!context.Request.Query.ContainsKey(CommonValueConstant.LANGUAGE_QUERY_STRING_NAME))
            //            {
            //                var _workContext = DependencyInjectionHelper.GetService<IWorkContext>();
            //                int langCodeId = _workContext.ListLanguageCommon.Find(x => x.LanguageCode.ToLower() == langCode.ToString().ToLower()).Id;
            //                context.Request.QueryString.Add(CommonValueConstant.LANGUAGE_QUERY_STRING_NAME, langCodeId.ToString());
            //            }


            //        }
            //    }
            //    catch
            //    {

            //    }
            //    return next();
            //});

            return app;
        }
    }
}
