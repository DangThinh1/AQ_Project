using AQS.BookingMVC.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AQS.BookingMVC.Infrastructure.Mvc.Filters
{
    public class LanguguageParamsFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Query.ContainsKey(CommonValueConstant.LANGUAGE_QUERY_STRING_NAME))
            {
                context.Result = new RedirectToRouteResult("NotFound", null) ;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.
            //MyDebug.Write(MethodBase.GetCurrentMethod(), context.HttpContext.Request.Path);
        }
    }
   
}
