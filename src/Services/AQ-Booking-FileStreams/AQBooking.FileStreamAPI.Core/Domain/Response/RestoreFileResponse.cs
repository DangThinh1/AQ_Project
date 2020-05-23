using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStreamAPI.Core.Domain.Response
{
    public class RestoreFileResponse
    {
        public int TotalItems { get; set; }
        public int TotalItemSuccess { get; set; }
        public int TotalItemFail { get; set; }
        public string PathReport { get; set; }
    }
}
