using AQBooking.Core.Paging;

namespace YachtMerchant.Core.Models.YachtTourInformations
{
    public class YachtTourInformationSearchModel : SearchModel
    {
        public string Title { get; set; }
        public string IsActivated { get; set; }
        public int TourFid { get; set; }
        public string ActivatedDate { get; set; }
        public int TourInformationTypeFid { get; set; }
    }
}