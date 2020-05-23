using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantFileStreams
    {
        public int Id { get; set; }
        public int RestaurantFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public bool Deleted { get; set; }
        public DateTime ActivatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual Restaurants Restaurant { get; set; }
    }
}
