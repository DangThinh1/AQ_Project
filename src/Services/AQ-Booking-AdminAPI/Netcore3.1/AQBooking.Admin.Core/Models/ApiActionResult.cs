using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Core.Models
{
    public class ApiActionResult
    {
        public object ResponseData { get; protected set; }
        public bool Succeeded { get; protected set; }
        public List<ServiceError> Errors { get; }
        public ApiActionResult()
        {

        }
        public ApiActionResult(bool succeeded, object responseData = null)
        {
            Succeeded = succeeded;
            Errors = new List<ServiceError>();
            ResponseData = responseData;
        }

        public ApiActionResult(bool succeeded, List<ServiceError> errors, object responseData = null)
        {
            Succeeded = succeeded;
            Errors = errors;
            ResponseData = responseData;
        }

        public static async Task<ApiActionResult> SuccessAsync(object responseData = null)
        {
            var result = new ApiActionResult(true, responseData);
            return await Task.FromResult(result);
        }
        public static ApiActionResult SuccessData(object responseData = null) => new ApiActionResult(true, responseData);
        public static ApiActionResult Success()
        {
            var result = new ApiActionResult(true);
            return result;
        }

        public static ApiActionResult Failed(List<ServiceError> errors)
        {
            var result = new ApiActionResult(false, errors);
            return result;
        }
        public static ApiActionResult Failed(ServiceError error)
        {
            var errors = new List<ServiceError>() { error };
            var result = new ApiActionResult(false, errors);
            return result;
        }
        public static ApiActionResult Failed(string error)
        {
            var errors = new List<ServiceError>() {
                new ServiceError("", error)
            };
            var result = new ApiActionResult(false, errors);
            return result;
        }

        public static async Task<ApiActionResult> FailedAsync(List<ServiceError> errors)
        {
            var result = new ApiActionResult(false, errors);
            return await Task.FromResult(result);
        }
        public static async Task<ApiActionResult> FailedAsync(ServiceError error)
        {
            var errors = new List<ServiceError>() { error };
            var result = new ApiActionResult(false, errors);
            return await Task.FromResult(result);
        }
        public static async Task<ApiActionResult> FailedAsync(string error)
        {
            var errors = new List<ServiceError>() {
                new ServiceError("", error)
            };
            var result = new ApiActionResult(false, errors);
            return await Task.FromResult(result);
        }

        public bool HasErrorMessage()
        {
            if (Errors != null && Errors.FirstOrDefault() != null &&
                !string.IsNullOrEmpty(Errors.FirstOrDefault().Description))
            {
                return true;
            }
            return false;
        }
        public string GetFirstErrorMessage()
        {
            if (Errors != null && Errors.FirstOrDefault() != null)
            {
                return Errors.FirstOrDefault().Description;
            }
            return string.Empty;
        }
        public string GetFirstErrorCode()
        {
            if (Errors != null && Errors.FirstOrDefault() != null)
            {
                return Errors.FirstOrDefault().Code;
            }
            return string.Empty;
        }
    }

    public class ServiceError
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public ServiceError(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
