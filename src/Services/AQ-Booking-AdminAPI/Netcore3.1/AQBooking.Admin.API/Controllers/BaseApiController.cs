using AQBooking.Admin.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AQBooking.Admin.API.Controllers
{
    public class BaseApiController: ControllerBase
    {
        protected  IActionResult OkBaseResponse<T>(T data)
        {
            var response = new BaseResponse<T>
            {
                ResponseData = data,
                StatusCode = System.Net.HttpStatusCode.OK,

            };
            return Ok(response);
        }
        protected IActionResult ErrorBaseResponse(string message)
        {
            var response = new BaseResponse<string>
            {
                
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message=message

            };
            return Ok(response);
        }
        protected IActionResult ErrorBaseResponse(Exception ex)
        {
            var response = new BaseResponse<string>
            {

                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Message = ex.Message,
                FullMsg=ex.ToString()

            };
            return Ok(response);
        }
        protected IActionResult ErrorBaseResponse(HttpStatusCode statusCode,string errorMsg=null)
        {
            var response = new BaseResponse<string>
            {

                StatusCode = statusCode,
                Message = errorMsg,               

            };
            return Ok(response);
        }
      
    }
}
