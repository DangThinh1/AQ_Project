using APIHelpers.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APIHelpers
{
   public class APIResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public APIResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsSwagger(context))
                await this._next(context);
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        await _next.Invoke(context);

                        if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            var body = await FormatResponse(context.Response);
                            await HandleSuccessRequestAsync(context, body, context.Response.StatusCode);
                        }
                        else
                        {
                            await HandleNotSuccessRequestAsync(context, context.Response.StatusCode);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        await HandleExceptionAsync(context, ex);
                    }
                    finally
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                }
            }

        }

        private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            int code = 0;

            BaseResponse<string> apiResponse = new BaseResponse<string> {
                
            };
         
           if (exception is UnauthorizedAccessException)
            {
                apiResponse.Message = "Unauthorized Access";
                code = (int)HttpStatusCode.Unauthorized;
                apiResponse.StatusCode = HttpStatusCode.Unauthorized;
                context.Response.StatusCode = code;
            }
            else
            {
#if !DEBUG
                var msg = "An unhandled error occurred.";  
                string stack = null;  
#else
                var msg = exception.GetBaseException().Message;
                string stack = exception.StackTrace;
#endif

                apiResponse.Message = stack??msg;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                code = (int)HttpStatusCode.InternalServerError;
                context.Response.StatusCode = code;
            }

            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(apiResponse);

            return context.Response.WriteAsync(json);
        }

        private static Task HandleNotSuccessRequestAsync(HttpContext context, int code)
        {
            context.Response.ContentType = "application/json";

            BaseResponse<string> apiResponse = new BaseResponse<string> {
                StatusCode=(HttpStatusCode)code
            };

            if (code == (int)HttpStatusCode.NotFound)
                apiResponse.Message = "The specified URI does not exist. Please verify and try again.";
            else if (code == (int)HttpStatusCode.NoContent)
                apiResponse.Message = "The specified URI does not contain any content.";
            else
                apiResponse.Message= "Your request cannot be processed. Please contact a support.";
         
            context.Response.StatusCode = code;
            var json = JsonConvert.SerializeObject(apiResponse);
            return context.Response.WriteAsync(json);
        }

        private static Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
        {
            context.Response.ContentType = "application/json";
            string jsonString, bodyText = string.Empty;
            BaseResponse<object> apiResponse = new BaseResponse<object>
            {
                StatusCode = (HttpStatusCode)code
            };


            //if (!body.ToString().IsValidJson())
            //    bodyText = JsonConvert.SerializeObject(body);
            //else
                bodyText = body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type type;

            type = bodyContent?.GetType();

            if (bodyText!=""&& type.Equals(typeof(Newtonsoft.Json.Linq.JObject)))
            {
                apiResponse.ResponseData = JsonConvert.DeserializeObject<object>(bodyText);                
            }
            else
            {
                apiResponse.ResponseData = bodyContent;               
               
            }
            jsonString = JsonConvert.SerializeObject(apiResponse);
            return context.Response.WriteAsync(jsonString);
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
           
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return plainBodyText;
        }

        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");

        }
    }
}
