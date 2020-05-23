using AccommodationMerchant.Api.ExceptionExtentions;
using APIHelpers.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Net;

namespace AccommodationMerchant.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            //Initialization and set default error status code is 500
            BaseResponse<string> apiResponse = new BaseResponse<string>
            {
                StatusCode = statusCode
            };

            //Error not found
            if (context.Exception is EntityNotFoundException)
            {
                apiResponse.StatusCode = HttpStatusCode.NotFound;
            }

            //Error unauthorized
            if (context.Exception is UnauthorizedAccessException)
            {
                apiResponse.Message = "Unauthorized";
                apiResponse.StatusCode = HttpStatusCode.Unauthorized;
            }

            context.HttpContext.Response.ContentType = "application/json";

            apiResponse.Message = context.Exception.Message;
            apiResponse.FullMsg = context.Exception.StackTrace;

            var json = JsonConvert.SerializeObject(apiResponse);

            context.HttpContext.Response.WriteAsync(json, context.HttpContext.RequestAborted);
        }
    }
}
