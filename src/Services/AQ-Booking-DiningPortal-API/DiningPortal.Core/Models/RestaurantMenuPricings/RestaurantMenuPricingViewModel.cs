using System;

namespace AQDiningPortal.Core.Models.RestaurantMenuPricings
{
    public class RestaurantMenuPricingViewModel
    {
        //for test
        public DateTime EffectiveDate { get; set; }

        public bool HasPrice { get; set; }
        public bool Discounted { get; set; }
        public double? Price { get; set; }
        public double? OriginalPrice { get; set; }
        public double? DiscountedValue { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public string Remark { get; set; }
    }
}
