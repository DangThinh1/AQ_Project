using System;

namespace Identity.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Message { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public bool ShowMessage => !string.IsNullOrEmpty(Message);
    }
}