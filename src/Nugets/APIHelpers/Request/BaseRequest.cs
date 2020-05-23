using System;
using System.Collections.Generic;
using System.Text;

namespace APIHelpers.Request
{
    public class BaseRequest<TRequest> 
    {
        public BaseRequest()
        {

        }
        public BaseRequest(TRequest _requestData)
        {
            RequestData = _requestData;
        }
        public TRequest RequestData { get; set; }
    }
}
