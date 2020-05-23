using System.Collections.Generic;
using YachtMerchant.Core.Models.Yacht;

namespace YachtMerchant.Core.Models.YachtTours
{
    public class YachTourDetailModel
    {
        public YachTourViewModel Tour { get; set; }
        public List<YachtBasicProfileModel> ListYachts { get; set; }
        public int TourCharterCount { get; set; }
    }
}