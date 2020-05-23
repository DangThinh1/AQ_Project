using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;

namespace AQBooking.Admin.API.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                var firstErrorField = actionContext.ModelState
                                            .Keys
                                            .First(x => actionContext.ModelState[x].Errors.Count > 0);
                var firstErrorMessage = actionContext.ModelState
                                            .Values.First(n => n.Errors.Count > 0)
                                            .Errors[0].ErrorMessage;

                var message = !string.IsNullOrEmpty(firstErrorMessage)
                                ? $"{firstErrorField}: {firstErrorMessage}"
                                : actionContext.ModelState.Keys.First();

                actionContext.Result = new BadRequestObjectResult(message);
                base.OnActionExecuting(actionContext);
            }
        }
    }
}
