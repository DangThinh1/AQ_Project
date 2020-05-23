using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Identity.Helpers
{
    public static class ResponseHelper
    {
        private const string SUCCESS = "Success.";
        private const string BAD_REQUEST = "Bad request.";
        private const string UNAUTHORIZED = "Unauthorized.";
        private const string INTERNAL_SERVER_ERROR = "Internal Server Error.";
        private const string CONFLICT = "Conflict.";

        public static async Task<IActionResult> OkAsync(this ControllerBase controller, object data = null, string message = SUCCESS)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = message
            }));
        }

        public static async Task<IActionResult> BadRequestAsync(this ControllerBase controller, string message = BAD_REQUEST, object data = null)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = message
            }));
        }

        public static async Task<IActionResult> UnauthorizedAsync(this ControllerBase controller, string message = UNAUTHORIZED, object data = null)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Message = message
            }));
        }

        public static async Task<IActionResult> ConflictAsync(this ControllerBase controller, string message = CONFLICT, object data = null)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.Conflict,
                Message = message
            }));
        }

        public static async Task<IActionResult> InternalServerErrorAsync(this ControllerBase controller, string message = INTERNAL_SERVER_ERROR, object data = null)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Message = message
            }));
        }
    }
}
