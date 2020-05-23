using AQBooking.Core.Paging;
using System;

namespace YachtMerchant.Core.Models.YachtAttribute
{
    public class YachtAttributeSearchModel : SearchModel
    {
        public int? AttributeCategoryFid { get; set; }

        public YachtAttributeSearchModel():base()
        {
            AttributeCategoryFid = null;
        }
    }
}
