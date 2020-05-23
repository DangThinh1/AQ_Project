using System;

namespace AQDiningPortal.Core.Models.RestaurantMenuInfoDetails
{
    public class RestaurantMenuInfoDetailViewModel
    {
        //for test
        public DateTime EffectiveDate { get; set; }

        public int LanguageFid { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public bool UsingSpecialImage { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
    }
}
