using System;
using System.Collections.Generic;

namespace Accommodation.Infrastructure.Databases.Entities
{
    public partial class HotelInventories
    {
        public long Id { get; set; }
        public int HotelFid { get; set; }
        public string RoomName { get; set; }
        public int RoomTypeFid { get; set; }
        public string RoomTypeResKey { get; set; }
        public string HotelRoomCode { get; set; }
        public int Quantity { get; set; }
        public double RoomSizeSqm { get; set; }
        public double RoomSizeSqft { get; set; }
        public int MaxOccupiedPerson { get; set; }
        public int BedQuantity { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public Guid? ActivatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}