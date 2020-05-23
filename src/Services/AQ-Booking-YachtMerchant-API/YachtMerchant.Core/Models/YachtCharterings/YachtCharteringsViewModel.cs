using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharterings
{
    public class YachtCharteringsViewModel
    {
        public long Id { get; set; }
        public int YachtFid { get; set; }
        public string YachtName { get; set; }
        public int SourceFid { get; set; }
        public string SourceName { get; set; }
        public string SourceResKey { get; set; }
        public string UniqueId { get; set; }
        public string CustomerName { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public bool IsExistingCustomer { get; set; }
        public Guid? CustomerFid { get; set; }
        public int Passengers { get; set; }
        public int? CountItem { get; set; } = 1; // relation 1:1 : Count total item in detail
        public string CustomerNote { get; set; }  
        public DateTime CharterDateFrom { get; set; }
        public DateTime CharterDateTo { get; set; }
        public DateTime? BookingDate { get; set; }
        public int YachtPortFid { get; set; }
        public string YachtPortName { get; set; }
        public bool HaveAdditionalServices { get; set; }
        public bool HaveCrewsMember { get; set; }
        public bool HaveChef { get; set; }
        public int PricingTypeFid { get; set; }
        public string PricingType { get; set; }
        public string PricingTypeResKey { get; set; }
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

       
    }
}
