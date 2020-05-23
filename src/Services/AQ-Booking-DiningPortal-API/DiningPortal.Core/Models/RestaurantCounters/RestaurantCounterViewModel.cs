namespace AQDiningPortal.Core.Models.RestaurantCounters
{
    public class RestaurantCounterViewModel
    {
        //public int Id { get; set; }
        //public int RestaurantFid { get; set; }
        public int TotalViews { get; set; }
        public int TotalReservations { get; set; }
        public int TotalSuccessReservations { get; set; }
        public int TotalReviews { get; set; }
        public int TotalRecommendeds { get; set; }
        public int TotalNotRecommendeds { get; set; }
    }
}
