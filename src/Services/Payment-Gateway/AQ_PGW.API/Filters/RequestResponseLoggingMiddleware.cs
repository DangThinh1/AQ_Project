using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Infrastructure.Servives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ_PGW.API.Filters
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private ISystemLogsServiceRepository _systemLogService;
        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                ILoggerFactory loggerFactory)
        {
            _next = next;

            _logger = loggerFactory
                      .CreateLogger<RequestResponseLoggingMiddleware>();
        }
        public async Task Invoke(HttpContext context, ISystemLogsServiceRepository systemLogService)
        {
            _systemLogService = systemLogService;
            //_logger.LogInformation(await FormatRequestAndSaveLog(context.Request));

            var idLogs = await FormatRequestAndSaveLog(context.Request);
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                _logger.LogInformation(await FormatResponseAndUpdateLogs(context.Response, idLogs));
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<decimal> FormatRequestAndSaveLog(HttpRequest request)
        {
            //https://gist.github.com/elanderson/c50b2107de8ee2ed856353dfed9168a2 Fix post Data
            request.EnableRewind();
            var body = request.Body;     

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            var url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request);

            SystemLogs log = new SystemLogs();
            log.LogDate = DateTime.Now;
            log.UrlRequest = url;
            log.DataURL = request.QueryString.ToString();
            log.DataBody = bodyAsText;
            log.TypeRequest = request.Method;
            if (request.Path.ToString().IndexOf("ProcessPayment") != -1 && request.QueryString.ToString().LastIndexOf("PAYPAL") != -1)
            {
                log.TypePayment = "PAYPAL";
            }
            else if (request.Path.ToString().IndexOf("ProcessPayment") != -1 && request.QueryString.ToString().LastIndexOf("STRIPE") != -1)
            {
                log.TypePayment = "STRIPE";
            }
            await _systemLogService.InsertSystemLogsAsync(log);

            //return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
            return log.ID;
        }

        private async Task<string> FormatResponseAndUpdateLogs(HttpResponse response, decimal id)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            var DataLogs = _systemLogService.GetSystemLogsById(id);
            DataLogs.DataReponse = text;
            DataLogs.StatusCode= response.StatusCode;
            _systemLogService.UpdateSystemLogs(DataLogs);
            return $"Response {text}";
        }
    }
}
