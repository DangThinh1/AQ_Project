using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Core.Models
{
    public class BasicResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public string ResourceKey { get; set; }

        public BasicResponse(bool isSucceed, string message, string resouceKey)
        {
            IsSucceed = isSucceed;
            Message = message;
            ResourceKey = resouceKey;
        }

        public static BasicResponse Succeed(string message, string resouceKey = "")
        {
            return new BasicResponse(true, message, resouceKey);
        }

        public static BasicResponse Failed(string message, string resouceKey = "")
        {
            return new BasicResponse(false, message, resouceKey);
        }
    }
}
