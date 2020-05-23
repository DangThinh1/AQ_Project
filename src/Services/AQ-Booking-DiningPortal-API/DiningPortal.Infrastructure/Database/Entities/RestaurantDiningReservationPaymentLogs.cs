using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantDiningReservationPaymentLogs
    {
        public long Id { get; set; }
        public int ReservationsFid { get; set; }
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
