using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantInformation
{
    public class YachtMerchantInformationSearchModel : SearchModel
    {
        public string Title { get; set; }
        public string IsActivated { get; set; }
        public int MerchantFid { get; set; }
        public string ActivatedDate { get; set; }
    }
}
