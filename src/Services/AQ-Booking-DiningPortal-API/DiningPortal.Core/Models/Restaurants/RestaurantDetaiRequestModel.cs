using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Core.Models.Restaurants
{
    public class RestaurantDetaiRequestModel
    {
        public int RestaurantId { get; set; }
        public int LanguageId { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}
