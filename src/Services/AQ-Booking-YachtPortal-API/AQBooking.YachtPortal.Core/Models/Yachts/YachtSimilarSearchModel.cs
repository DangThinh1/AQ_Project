using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class YachtSimilarSearchModel
    {
        public int NumOfPassenger { get; set; }
        public string CheckOut { get; set; }
        public string CheckIn { get; set; }

        public double Price { get; set; }
        public int PricingTypeId { get; set; }
   
        public string City { get; set; }
        public int ExcludeYachtID { get; set; }
    }
}
