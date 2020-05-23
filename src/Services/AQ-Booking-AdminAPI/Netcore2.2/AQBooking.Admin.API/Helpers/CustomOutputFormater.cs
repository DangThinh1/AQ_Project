using AQBooking.Admin.Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.API.Helpers
{
    public class CustomOutputFormater : TextOutputFormatter
    {
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings
           = new JsonSerializerSettings
           {
               ContractResolver = new CamelCasePropertyNamesContractResolver(),
               NullValueHandling = NullValueHandling.Ignore,
           };

        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        private readonly Dictionary<Type, object> _templates;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public CustomOutputFormater(Dictionary<Type, object> templates)
           : this(templates, DefaultJsonSerializerSettings, DefaultEncoding)
        {

        }

        public CustomOutputFormater(Dictionary<Type, object> templates, JsonSerializerSettings settings)
            : this(templates, settings, DefaultEncoding)
        {
        }

        public CustomOutputFormater(Dictionary<Type, object> templates, Encoding encoding)
            : this(templates, DefaultJsonSerializerSettings, encoding)
        {
        }

        public CustomOutputFormater(Dictionary<Type, object> templates, JsonSerializerSettings settings, Encoding encoding)
        {
            _templates = templates;
            _jsonSerializerSettings = settings;
            SupportedEncodings.Add(encoding);
        }
        public CustomOutputFormater()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            //var template = _templates[context.ObjectType];
            //var builder = template.GetType()
            //    .GetMethod("ToRepresentation")
            //    .Invoke(template, new[] { context.Object });

            //var entity = builder.GetType().GetMethod("Build").Invoke(builder, new object[0]);

            try
            {

                if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK)
                {

                    return HandleSuccessRequestAsync(context.HttpContext, context.Object, context.HttpContext.Response.StatusCode);

                }
                else
                {
                    return HandleNotSuccessRequestAsync(context.HttpContext, context.HttpContext.Response.StatusCode, context.Object);
                }
            }
            catch (System.Exception ex)
            {
                return HandleExceptionAsync(context.HttpContext, ex);
            }

        }
        private static Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {

            BaseResponse<string> apiResponse = new BaseResponse<string>
            {

            };
            context.Response.StatusCode = 200;
            if (exception is UnauthorizedAccessException)
            {
                apiResponse.Message = "Unauthorized Access";
                apiResponse.StatusCode = HttpStatusCode.Unauthorized;

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

                apiResponse.Message = stack ?? msg;
                apiResponse.StatusCode = HttpStatusCode.InternalServerError;

            }

            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(apiResponse);

            return context.Response.WriteAsync(json, context.RequestAborted);
        }

        private static Task HandleNotSuccessRequestAsync(HttpContext context, int code, object body)
        {
            context.Response.ContentType = "application/json";

            BaseResponse<string> apiResponse = new BaseResponse<string>
            {
                StatusCode = (HttpStatusCode)code
            };

            if (code == (int)HttpStatusCode.NotFound)
                apiResponse.Message = "The specified URI does not exist. Please verify and try again.";
            else if (code == (int)HttpStatusCode.NoContent)
                apiResponse.Message = "The specified URI does not contain any content.";
            else if (code == (int)HttpStatusCode.BadRequest)
            {
                var exception = body as Exception;
                apiResponse.Message = exception.Message;
#if DEBUG
                //apiResponse.FullMsg = exception.ToString();
#endif
            }
            else
                apiResponse.Message = "Your request cannot be processed. Please contact a support.";


            context.Response.StatusCode = 200;
            var json = JsonConvert.SerializeObject(apiResponse);
            //return context.HttpContext.Response.WriteAsync(
            //   JsonConvert.SerializeObject(entity, _jsonSerializerSettings),
            //   selectedEncoding,
            //   context.HttpContext.RequestAborted);
            return context.Response.WriteAsync(json, context.RequestAborted);
        }

        private static Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
        {
            context.Response.ContentType = "application/json";
            string jsonString, bodyText = string.Empty;
            BaseResponse<object> apiResponse = new BaseResponse<object>
            {
                StatusCode = (HttpStatusCode)code
            };
            apiResponse.ResponseData = body;
            jsonString = JsonConvert.SerializeObject(apiResponse);
            return context.Response.WriteAsync(jsonString, context.RequestAborted);
        }
    }
}
