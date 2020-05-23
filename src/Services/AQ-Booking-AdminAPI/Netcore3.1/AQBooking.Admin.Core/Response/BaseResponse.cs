using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace AQBooking.Admin.Core.Response
{
    public class BaseResponse<TResponse>
    {
        public HttpStatusCode StatusCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }
        public TResponse ResponseData { get; set; }
        public object ResponseHeader { get; set; }
        public string FullResponseString { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FullMsg { get; set; }
        public bool IsSuccessStatusCode => StatusCode == HttpStatusCode.OK;

        public string AQStatusCode { get; set; }
        public string ResourceKey { get; set; }
    }
}

