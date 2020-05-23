using System;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtTourCharterProcessingFees
    {
        public long TourCharterFid { get; set; }
        public double ProcessingValue { get; set; }
        public double Percentage { get; set; }
        public double ProcessingFee { get; set; }
        public string Remark { get; set; }
        public DateTime GeneratedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}