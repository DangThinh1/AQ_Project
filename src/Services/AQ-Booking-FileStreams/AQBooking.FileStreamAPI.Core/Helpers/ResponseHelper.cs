using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AQBooking.FileStreamAPI.Core.Helpers
{
    public static class ResponseHelper
    {
        private const string SUCCESS = "Success.";
        private const string BAD_REQUEST = "Bad request.";
        private const string UNAUTHORIZED = "Unauthorized.";
        private const string NO_CONTENT = "No Content.";
        private const string INTERNAL_SERVER_ERROR = "Internal Server Error.";
        private const string CONFLICT = "Conflict.";
        private const string NOT_FOUND = "Not Found!";
        private const string CREATED = "Create new data";

        public static async Task<IActionResult> OkAsync(this ControllerBase controller, object data = null, object header = null, string message = SUCCESS)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = message,
                ResponseHeader = header
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

        public static async Task<IActionResult> NoContentAsync(this ControllerBase controller, string message = NO_CONTENT, object data = null)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.NoContent,
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

        public static async Task<IActionResult> NotFoundAsync(this ControllerBase controller, object data = null, string message = NOT_FOUND)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Message = message
            }));
        }
        public static async Task<IActionResult> CreatedAsync(this ControllerBase controller, object data = null, string message = CREATED)
        {
            return await Task.Run(() => controller.Ok(new BaseResponse<object>()
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.Created,
                Message = message
            }));
        }
    }
}
