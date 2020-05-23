using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Areas.Yacht.Models
{
    public class YachtSimilarModel
    {
        public string YatchId { get; set; }
        public string CheckOut { get; set; }
        public string CheckIn { get; set; }       
       
        public double Price { get; set; }
        public int PricingTypeId { get; set; }        
        public string City { get; set; }
        public int NumOfPassenger { get; set; }


    }
}
