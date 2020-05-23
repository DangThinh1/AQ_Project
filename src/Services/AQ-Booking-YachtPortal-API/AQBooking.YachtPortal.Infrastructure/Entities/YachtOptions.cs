using System;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtOptions
    {
        public int YachtId { get; set; }
        public string YachtUniqueId { get; set; }
        public bool IsExclusiveYacht { get; set; }
        public bool AutoCancelledPromotion { get; set; }
        public double CancelledPromotionPercent { get; set; }
        public bool HaveAdditionalServices { get; set; }
        public bool ActiveForTour { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public virtual Yachts Yacht { get; set; }
    }
}