using Accommodation.Core.Models.HotelReservationDetails;
using System.Collections.Generic;
using System;

namespace Accommodation.Core.Models.HotelReservations
{
    public class HotelReservationViewModel
    {
        public long Id { get; set; }
        public int HotelFid { get; set; }        
        public string CustomerName { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public bool IsExistingCustomer { get; set; }
        public Guid? CustomerFid { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public string CustomerNote { get; set; }
        public DateTime? DiningDate { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public double OriginalValue { get; set; }
        public double DiscountedValue { get; set; }
        public double GrandTotalValue { get; set; }
        public double PrepaidRate { get; set; }
        public double PrepaidValue { get; set; }
        public string PaymentCurrency { get; set; }
        public double PaymentExchangeRate { get; set; }
        public DateTime? PaymentExchangeDate { get; set; }
        public double PaymentValue { get; set; }
        public bool GotSpecialRequest { get; set; }
        public string SpecialRequestDescriptions { get; set; }
        public string CancelReason { get; set; }
        public int StatusFid { get; set; }
        public string StatusResKey { get; set; }
        public bool Processed { get; set; }
        public Guid? ProcessedBy { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string ProcessedRemark { get; set; }
        public List<HotelReservationDetailViewModel> hotelReserDetailLst { get; set; }
    }
}
