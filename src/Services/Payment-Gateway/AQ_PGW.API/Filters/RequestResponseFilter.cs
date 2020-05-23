using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQ_PGW.API.Filters
{
    public class RequestResponseFilter : IActionFilter
    {
        private ISystemLogsServiceRepository _systemLogService;
        public RequestResponseFilter(ISystemLogsServiceRepository systemLogService)
        {
            _systemLogService = systemLogService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executes
            LoggingRequest(context.HttpContext.Request);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }

        private async Task<bool> LoggingRequest(HttpRequest request)
        {
            //var url = request.HttpContext.Request.GetEncodedUrl();
            //var url = request.HttpContext.Request..GetDisplayUrl();
            var url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request);
            if (url.Contains("api/") == false)
                return false;

            SystemLogs log = new SystemLogs();
            log.LogDate = DateTime.Now;
            log.UrlRequest = url;
            log.DataURL = ConvertCollectionParamsToString(request);

            await _systemLogService.InsertSystemLogsAsync(log);

            return log.ID != 0;
        }

        private string ConvertCollectionParamsToString(HttpRequest request)
        {
            string data = string.Empty;
            try
            {
                data = request.QueryString.Value;
            }
            catch (Exception ex) { }

            return data;
        }
    }
}
