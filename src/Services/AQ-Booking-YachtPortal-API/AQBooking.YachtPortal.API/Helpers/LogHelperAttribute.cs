using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AQBooking.YachtPortal.API.Helpers
{
    public class LogHelperAttribute : ActionFilterAttribute
    {
        private bool _logParameter;
        private bool _logResult;
        private const string LOG_PARAM_LABEL = "Log Parameter";
        private const string LOG_RESULT_LABEL = "Log Result";

        public LogHelperAttribute(bool logParameter = true, bool logResult = false)
        {
            _logParameter = logParameter;
            _logResult = logResult;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            try
            {
                if (_logParameter)
                {
                    string controller = context.Controller.ToString();
                    string action = context.ActionDescriptor.DisplayName;
                    var parameter = JsonConvert.SerializeObject(context.ActionArguments);
                }
            }
            catch
            {
                return;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            try
            {
                if (_logResult)
                {
                    string controller = context.Controller.ToString();
                    string action = context.ActionDescriptor.DisplayName;
                    var result = JsonConvert.SerializeObject(context.Result);
                }
            }
            catch
            {
                return;
            }

        }
    }
}
