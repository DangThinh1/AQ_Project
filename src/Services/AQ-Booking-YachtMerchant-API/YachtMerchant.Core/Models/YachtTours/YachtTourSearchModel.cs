using AQBooking.Core.Paging;

namespace YachtMerchant.Core.Models.YachtTours
{
    public class YachtTourSearchModel : PagableModel
    {
        public string TourName { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
    }
}