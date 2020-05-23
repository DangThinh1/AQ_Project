using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharterings
{
    public class YachtCharteringsCreateModel
    {
        [Required]
        public int YachtFid { get; set; }
        [Required]
        public int SourceFid { get; set; }
        public string SourceResKey { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        [Required]
        public bool IsExistingCustomer { get; set; }
        public Guid? CustomerFid { get; set; }
        [Required]
        public int Passengers { get; set; }
        public string CustomerNote { get; set; }  
        [Required]
        public DateTime CharterDateFrom { get; set; }
        [Required]
        public DateTime CharterDateTo { get; set; }
        [Required]
        public int YachtPortFid { get; set; }
        public string YachtPortName { get; set; }
        [Required]
        public bool HaveAdditionalServices { get; set; }
        [Required]
        public bool HaveCrewsMember { get; set; }
        [Required]
        public bool HaveChef { get; set; }
        [Required]
        public int PricingTypeFid { get; set; }
        public string PricingTypeResKey { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        [Required]
        public double OriginalValue { get; set; }
        [Required]
        public double DiscountedValue { get; set; }
        [Required]
        public double GrandTotalValue { get; set; }
        [Required]
        public double PrepaidRate { get; set; }
        [Required]
        public double PrepaidValue { get; set; }
        public string PaymentCurrency { get; set; }
        public double PaymentExchangeRate { get; set; }
        public double PaymentValue { get; set; }
        public bool GotSpecialRequest { get; set; }
        public string SpecialRequestDescriptions { get; set; }
        public string CancelReason { get; set; }
        public int StatusFid { get; set; }
        public string StatusResKey { get; set; }


    }
}
