
namespace AccommodationMerchant.Core.Models.HotelReservationDetails
{
    public  class HotelReservationDetailModel
    {
        public long Id { get; set; }
        public int HotelFid { get; set; }
        public string HotelUniqueId { get; set; }
        public string HotelName { get; set; }
        public long ReservationsFid { get; set; }
        public long InventoryFid { get; set; }
        public long InvetoryRateFid { get; set; }
        public string RoomName { get; set; }
        public int RoomTypeFid { get; set; }
        public string RoomTypeResKey { get; set; }
        public string HotelRoomCode { get; set; }
        public int Quantity { get; set; }
        public double RoomSizeSqm { get; set; }
        public double RoomSizeSqft { get; set; }
        public int MaxOccupiedPerson { get; set; }
        public int BedQuantity { get; set; }
        public bool HasPrice { get; set; }
        public bool Discounted { get; set; }
        public bool WithBreakfast { get; set; }
        public string ItemName { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public double OriginalValue { get; set; }
        public double DiscountedValue { get; set; } = 0;
        public double FinalValue { get; set; }
        public int OrderAmount { get; set; }
        public double GrandTotalValue { get; set; }
        public string Remark { get; set; }
    }
}
