using System;
using System.Collections.Generic;

namespace Accommodation.Infrastructure.Databases.Entities
{
    public partial class HotelReservationProcessingFees
    {
        public long ReservationsFid { get; set; }
        public double ProcessingValue { get; set; }
        public double Percentage { get; set; }
        public double ProcessingFee { get; set; }
        public string Remark { get; set; }
        public DateTime GeneratedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}