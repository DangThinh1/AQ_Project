using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.FileStream.Core.Models.FileResponse
{
    public class RestoreFileResponse
    {
        public int TotalItems { get; set; }
        public int TotalItemSuccess { get; set; }
        public int TotalItemFail { get; set; }
        public string PathReport { get; set; }
    }
}
