using System;
using System.Collections.Generic;
using System.Text;

namespace Accommodation.Core.Models.HotelReservationDetails
{
    public class HotelReservationDetailViewModel
    {
        public long Id { get; set; }
        public int HotelFid { get; set; }
        public long ReservationsFid { get; set; }
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
