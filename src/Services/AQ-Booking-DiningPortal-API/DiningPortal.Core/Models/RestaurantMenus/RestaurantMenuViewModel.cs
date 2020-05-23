using AQDiningPortal.Core.Models.RestaurantMenuInfoDetails;
using AQDiningPortal.Core.Models.RestaurantMenuPricings;

namespace AQDiningPortal.Core.Models.RestaurantMenus
{
    public class RestaurantMenuViewModel
    {
        public int Id { get; set; }
        //public int RestaurantFid { get; set; }
        public string UniqueId { get; set; }
        public string DefaultName { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public int ServingPortions { get; set; }
        public double MinimumTimeToOrder { get; set; }
        public bool IsSignatureDish { get; set; }
        public double? OrderBy { get; set; }

        public RestaurantMenuPricingViewModel Pricings { get; set; }
        public RestaurantMenuInfoDetailViewModel InfoDetails { get; set; }
    }
}
