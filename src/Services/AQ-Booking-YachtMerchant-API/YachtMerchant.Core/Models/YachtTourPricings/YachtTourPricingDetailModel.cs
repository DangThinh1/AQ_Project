using System.Collections.Generic;
using YachtMerchant.Core.Models.Yacht;

namespace YachtMerchant.Core.Models.YachtTourPricings
{
    public partial class YachtTourPricingDetailModel
    {
        public YachtBasicProfileModel Yacht { get; set; }
        public YachtTourPricingViewModel CurrentPricing { get; set; }
    }
}
