using System;
using System.Collections.Generic;
using System.Text;

namespace AQ_PGW.Common
{
    public class DTO
    {
        public static class PaymentMethod
        {
            public static string Full { get { return "Full"; } }
            public static string Partial { get { return "Partial"; } }
        }

        public static class PaymentStatus
        {
            public static string Fail { get { return "Fail"; } }
            public static string Success { get { return "Success"; } }
            public static string Processing { get { return "Processing"; } }
            public static string Completed { get { return "Completed"; } }
        }
        

        public class APIResponseData
        {
            public string Version { get; set; } = "1.0";
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public APIResponseResultData Result { get; set; } = new APIResponseResultData();
        }

        public class APIResponseResultData
        {
            public object Data { get; set; }
        }

        public class ErrorInfo
        {
            public string Useragent { get; set; }
            public string Username { get; set; }
            public string Section { get; set; }
            public Exception Exception { get; set; }
        }
    }
}
