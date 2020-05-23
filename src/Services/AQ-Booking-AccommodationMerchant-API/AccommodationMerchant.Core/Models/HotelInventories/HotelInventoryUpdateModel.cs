using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationMerchant.Core.Models.HotelInventories
{
    public class HotelInventoryUpdateModel
    {
        public long Id { get; set; }
        public string RoomName { get; set; }
        public int RoomTypeFid { get; set; }
        public string RoomTypeResKey { get; set; }
        public string HotelRoomCode { get; set; }
        public int Quantity { get; set; }
        public double RoomSizeSqm { get; set; }
        public double RoomSizeSqft { get; set; }
        public int MaxOccupiedPerson { get; set; }
        public int BedQuantity { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}