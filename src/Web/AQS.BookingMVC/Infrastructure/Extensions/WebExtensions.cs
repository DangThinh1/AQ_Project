using APIHelpers.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.Extensions
{
    public static class WebExtensions
    {
        /// <summary>
        /// Check and get data from BaseResponse<T>
        /// </summary>
        /// <typeparam name="T">type of Object data</typeparam>
        /// <param name="baseResponse">Base response model return from api</param>
        /// <returns>Object Data</returns>
        public static T GetDataResponse<T>(this BaseResponse<T> baseResponse) where T: new()
        {
            if (baseResponse == null || !baseResponse.IsSuccessStatusCode || baseResponse.ResponseData == null)
                return new T();
            return baseResponse.ResponseData;
        }

        public static bool IsValidResponse<T>(this BaseResponse<T> baseResponse)
        {
            return (baseResponse != null)
                && (baseResponse.IsSuccessStatusCode)
                && (baseResponse.ResponseData != null);
        }

        public static string FormatCurrency(this double value, string cultureCode="en-US")
        {
            return value.ToString("C", CultureInfo.GetCultureInfo(cultureCode));
        }
       
    }
}
