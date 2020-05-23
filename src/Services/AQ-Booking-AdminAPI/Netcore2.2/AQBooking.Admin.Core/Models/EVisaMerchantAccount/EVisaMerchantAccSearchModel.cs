using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.EVisaMerchantAccount
{
    public class EVisaMerchantAccSearchModel : SearchModel
    {
        public string UserFid { get; set; }
        public int MerchantFid { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }
}
