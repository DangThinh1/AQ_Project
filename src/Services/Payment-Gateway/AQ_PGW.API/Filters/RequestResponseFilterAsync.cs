using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ_PGW.API.Filters
{
    public class RequestResponseFilterAsync : IAsyncActionFilter
    {
        private ISystemLogsServiceRepository _systemLogService;
        public RequestResponseFilterAsync(ISystemLogsServiceRepository systemLogService)
        {
            _systemLogService = systemLogService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // do something before the action executes
            var request = context.HttpContext.Request;
            if (request.Path.StartsWithSegments(new PathString("/api")))
            {
                var stopWatch = Stopwatch.StartNew();
                var requestTime = DateTime.UtcNow;
                var requestBodyContent = await ReadRequestBody(request);
                var originalBodyStream = context.HttpContext.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    var response = context.HttpContext.Response;
                    response.Body = responseBody;
                    await next();
                    stopWatch.Stop();

                    string responseBodyContent = null;
                    responseBodyContent = await ReadResponseBody(response);
                    await responseBody.CopyToAsync(originalBodyStream);


                }
            }

            await LoggingRequest(context.HttpContext.Request);

            var resultContext = await next();
            // do something after the action executes; resultContext.Result will be set

        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableRewind();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }
        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
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
