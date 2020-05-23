using System;

namespace AccommodationMerchant.Core.Models.HotelInventories
{
    public class HotelInventoryCreateModel
    { 
        public int HotelFid { get; set; }
        public string RoomName { get; set; }
        public int RoomTypeFid { get; set; }
        public string HotelRoomCode { get; set; }
        public int Quantity { get; set; }
        public double RoomSizeSqm { get; set; }
        public double RoomSizeSqft { get; set; }
        public int MaxOccupiedPerson { get; set; }
        public int BedQuantity { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}