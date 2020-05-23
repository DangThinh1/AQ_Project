using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace AQDiningPortal.Infrastructure.Extensions
{
    public static class ApiResponseExtension
    {
        public static IActionResult BaseResponse(this ControllerBase controller, BaseResponse<object> data)
        {
            return controller.Ok(data);
        }

        public static IActionResult OkResponse(this ControllerBase controller, object data = null,
            string resourceKey = RESOURCEKEY.STATUSCODE_OK, string message = STATUS_MESSAGE.OK)
        {
            var baseResponse = new BaseResponse<object>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                AQStatusCode = AQSTATUSCODE.OK,
                ResponseData = data,
                ResourceKey = resourceKey,
                Message = message
            };
            return controller.Ok(baseResponse);
        }

        public static IActionResult BadRequestResponse(this ControllerBase controller,
            string resourceKey = RESOURCEKEY.STATUSCODE_BAD_REQUEST, string message = STATUS_MESSAGE.BAD_REQUEST)
        {
            var baseResponse = new BaseResponse<object>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ResourceKey = resourceKey,
                AQStatusCode = AQSTATUSCODE.BAD_REQUEST,
                Message = message
            };
            return controller.Ok(baseResponse);
        }

        public static IActionResult InternalServerErrorResponse(this ControllerBase controller,
            string resourceKey = RESOURCEKEY.STATUSCODE_INTERNAL_SERVER_ERROR, string message = STATUS_MESSAGE.INTERNAL_SERVER_ERROR)
        {
            var baseResponse = new BaseResponse<object>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ResourceKey = resourceKey,
                AQStatusCode = AQSTATUSCODE.INTERNAL_SERVER_ERROR,
                Message = message
            };
            return controller.Ok(baseResponse);
        }

        public static IActionResult NotFoundResponse(this ControllerBase controller,
            string resourceKey = RESOURCEKEY.AQSTATUSCODE_NOT_FOUND, string message = STATUS_MESSAGE.DATA_NOT_FOUND)
        {
            var baseResponse = new BaseResponse<object>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ResourceKey = resourceKey,
                AQStatusCode = AQSTATUSCODE.NOT_FOUND,
                Message = message
            };
            return controller.Ok(baseResponse);
        }

        public static IActionResult NoContentResponse(this ControllerBase controller,
            string resourceKey = RESOURCEKEY.AQSTATUSCODE_NO_CONTENT, string message = STATUS_MESSAGE.NO_CONTENT)
        {
            var baseResponse = new BaseResponse<object>()
            {
                StatusCode = System.Net.HttpStatusCode.NoContent,
                ResourceKey = resourceKey,
                AQStatusCode = AQSTATUSCODE.NO_CONTENT,
                Message = message
            };
            return controller.Ok(baseResponse);
        }
    }

    //Move to another place in future
    public static class AQSTATUSCODE
    {
        public const string OK = "200";
        public const string BAD_REQUEST = "400";
        public const string NOT_FOUND = "404";
        public const string INTERNAL_SERVER_ERROR = "500";
        public const string NO_CONTENT = "204";
    }

    //Move to another place in future
    public static class RESOURCEKEY
    {
        public const string STATUSCODE_OK = "STATUSCODE_OK";
        public const string STATUSCODE_BAD_REQUEST = "STATUSCODE_BAD_REQUEST";
        public const string STATUSCODE_INTERNAL_SERVER_ERROR = "STATUSCODE_INTERNAL_SERVER_ERROR";
        public const string AQSTATUSCODE_NOT_FOUND = "AQSTATUSCODE_NOT_FOUND";
        public const string AQSTATUSCODE_NO_CONTENT = "AQSTATUSCODE_NO_CONTENT";
    }

    //Move to another place in future
    public static class STATUS_MESSAGE
    {
        public const string OK = "Success";
        public const string BAD_REQUEST = "Bad Request";
        public const string INTERNAL_SERVER_ERROR = "Internal Server Error";
        public const string DATA_NOT_FOUND = "Data not found";
        public const string NO_CONTENT = "No Content";
    }
}
