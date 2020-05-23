using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtMerchantFileStream
{
    public class YachtMerchantFileStreamCreateModel
    {
        public int MerchantFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public DateTime ActivatedDate { get; set; }
    }
}
