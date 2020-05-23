using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachAdditionalServices
{
    public class YachAdditionalServiceSearchModel: SearchModel
    {
        public int MerchantFid { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public  int AdditionalServiceId { get; set; }
    }
}
