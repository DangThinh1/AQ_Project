using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantReservationFees
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int RestaurantFid { get; set; }
        public string RestaurantUniqueId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int FeeTypeFid { get; set; }
        public double FlatFee { get; set; }
        public double AdultFee { get; set; }
        public double ChildFee { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public bool IsActivated { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual Restaurants Restaurant { get; set; }
    }
}
