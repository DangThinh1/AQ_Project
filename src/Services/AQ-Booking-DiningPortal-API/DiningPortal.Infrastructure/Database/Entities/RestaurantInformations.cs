using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public class RestaurantInformations
    {
        public int Id { get; set; }
        public int RestaurantFid { get; set; }
        public string UniqueId { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public Guid? ActivatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual Restaurants Restaurant { get; set; }
        public virtual ICollection<RestaurantInformationDetails> InformationDetails { get; set; }

        public RestaurantInformations()
        {
            InformationDetails = new HashSet<RestaurantInformationDetails>();
        }
    }
}
