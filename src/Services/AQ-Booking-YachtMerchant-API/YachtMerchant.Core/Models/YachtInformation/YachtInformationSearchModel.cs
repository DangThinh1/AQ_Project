using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtInformation
{
    public class YachtInformationSearchModel: SearchModel
    {
        public string Title { get; set; }
        public string IsActivated { get; set; }
        public int YachtFid { get; set; }
        public string ActivatedDate { get; set; }
    }
}
