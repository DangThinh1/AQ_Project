using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Payments
{
    public class YachtTourPaymentModel
    {
        public int YachtTourId { get; set; }
        public int YachtId { get; set; }
        public DateTime DepartTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public int Passenger { get; set; }
        public double OriginalPrice { get; set; }
        public double Prepaid { get; set; }
        public UserInfoPaymentModel UserInfo { get; set; }
    }
}
