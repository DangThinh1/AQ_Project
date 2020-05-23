namespace AQBooking.YachtPortal.Core.Models.YachtOptions
{
    public class YachtOptionViewModel
    {
        public int YachtId { get; set; }
        public string YachtUniqueId { get; set; }
        public bool IsExclusiveYacht { get; set; }
        public bool AutoCancelledPromotion { get; set; }
        public double CancelledPromotionPercent { get; set; }
        public bool HaveAdditionalServices { get; set; }
        public bool ActiveForTour { get; set; }
    }
}
