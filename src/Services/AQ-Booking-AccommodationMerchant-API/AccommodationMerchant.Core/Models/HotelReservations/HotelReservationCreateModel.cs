using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccommodationMerchant.Core.Models.HotelReservations
{
    public class HotelReservationCreateModel
    {
        [Required]
        public int HotelFid { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string ReservationEmail { get; set; }
        [Required]
        public string ContactNo { get; set; }
        public bool IsExistingCustomer { get; set; }
        public Guid? CustomerFid { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public string CustomerNote { get; set; } = "";
        public DateTime? DiningDate { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public double OriginalValue { get; set; }
        public double DiscountedValue { get; set; }
        public double GrandTotalValue { get; set; }
        public string PaymentCurrency { get; set; }
        public double PaymentExchangeRate { get; set; }
        public double PaymentValue { get; set; }
        public bool GotSpecialRequest { get; set; }
        public string SpecialRequestDescriptions { get; set; }
        public int StatusFid { get; set; }
        public bool Processed { get; set; }

        public List<ReservationDetail> Details { get; set; }

    }

    public class ReservationDetail
    {
        public long ReservationsFid { get; set; }
        public int HotelFid { get; set; }
        public long InventoryFid { get; set; }
        public long InvetoryRateFid { get; set; }
        public string ItemName { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public double OriginalValue { get; set; }
        public double DiscountedValue { get; set; }
        public double FinalValue { get; set; }
        public int OrderAmount { get; set; }
        public double GrandTotalValue { get; set; }
        public string Remark { get; set; }
    }
}
