using System;

namespace AccommodationMerchant.Core.Models.HotelReservationPaymentLogs
{
    public class HotelReservationPaymentLogViewModel
    {
        public long Id { get; set; }
        public long ReservationFid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentBy { get; set; }
        public string PaymentRef { get; set; }
        public string PaymentMethod { get; set; }
        public double PaymentAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int StatusFid { get; set; }
        public string Remark { get; set; }
    }
}
