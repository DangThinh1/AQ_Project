using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtMerchantAccountMgt
{
    public class YachtMerchantAccMgtSearchModel : PagableModel
    {
        public string UserFid { get; set; }
        public int MerchantFid { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }
}
