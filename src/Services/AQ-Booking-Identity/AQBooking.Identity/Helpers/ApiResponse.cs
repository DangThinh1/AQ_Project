using AQBooking.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Identity.Helpers
{
    public class ApiResponse
    {
        public static ResponeModel Ok(object data = null)
        {
            return new ResponeModel()
            {
                StatusCode = 200,
                Message = "Success",
                Data = data
            };
        }

        public static ResponeModel BadRequest(string message = "Bad Request")
        {
            return new ResponeModel()
            {
                StatusCode = 400,
                Message = message,
                Data = null
            };
        }

        public static ResponeModel Conflict(string message = "Conflict")
        {
            return new ResponeModel()
            {
                StatusCode = 409,
                Message = message,
                Data = null
            };
        }

        public static ResponeModel Unauthorized(string message = "Unauthorized")
        {
            return new ResponeModel()
            {
                StatusCode = 401,
                Message = message,
                Data = null
            };
        }

        public static ResponeModel InternalServerError(string message = "Internal Server Error")
        {
            return new ResponeModel()
            {
                StatusCode = 500,
                Message = message,
                Data = null
            };
        }
    }
}
