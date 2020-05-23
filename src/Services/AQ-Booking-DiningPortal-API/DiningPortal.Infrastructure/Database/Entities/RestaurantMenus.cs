using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantMenus
    {
        public long Id { get; set; }
        public int RestaurantFid { get; set; }
        public string UniqueId { get; set; }
        public int CategoryFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string DefaultName { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public int ServingPortions { get; set; }
        public double MinimumTimeToOrder { get; set; }
        public bool IsSignatureDish { get; set; }
        public bool? IsActive { get; set; }
        public bool Deleted { get; set; }
        public double? OrderBy { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual Restaurants Restaurant { get; set; }
        public virtual ICollection<RestaurantMenuPricings> Pricings { get; set; }
        public virtual ICollection<RestaurantMenuInfoDetails> InfoDetails { get; set; }

        public RestaurantMenus()
        {
            Pricings = new HashSet<RestaurantMenuPricings>();
            InfoDetails = new HashSet<RestaurantMenuInfoDetails>();
        }
    }
}
