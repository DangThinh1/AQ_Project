using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Models.YachtBooking
{
    public class YachtBookingModel
    {
        public int YachtID { get; set; }
        public int Guest { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public double Price { get; set; }
        public YachtAttributeValueCharterPrivateGeneralViewModel Details { get; set; }

    }
}
