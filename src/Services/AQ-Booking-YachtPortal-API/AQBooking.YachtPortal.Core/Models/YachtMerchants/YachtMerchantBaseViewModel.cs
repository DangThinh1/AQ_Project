using AQBooking.YachtPortal.Core.Models.YachtMerchantFileStreams;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtMerchants
{
   public class YachtMerchantBaseViewModel
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int ZoneFid { get; set; }
        public string MerchantName { get; set; }
        public int LandingPageOptionFid { get; set; }

    }
}
