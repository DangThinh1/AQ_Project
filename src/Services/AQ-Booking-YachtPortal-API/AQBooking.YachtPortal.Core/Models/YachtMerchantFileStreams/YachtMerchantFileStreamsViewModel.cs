using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtMerchantFileStreams
{
   public class YachtMerchantFileStreamsViewModel
    {
        public long Id { get; set; }
        public int MerchantFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
    }
}
