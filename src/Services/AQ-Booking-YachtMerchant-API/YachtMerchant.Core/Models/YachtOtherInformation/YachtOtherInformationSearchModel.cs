using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtOtherInformation
{
    public class YachtOtherInformatioSearchModel: SearchModel
    {
        public string Title { get; set; }
        public string IsActivated { get; set; }
        public int YachtFid { get; set; }
        public string ActivatedDate { get; set; }
        public int LanguageId { get; set; }
    }
}
