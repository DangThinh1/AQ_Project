using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtFileStreams
{
    public class YachtFileStreamViewModel
    {
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
