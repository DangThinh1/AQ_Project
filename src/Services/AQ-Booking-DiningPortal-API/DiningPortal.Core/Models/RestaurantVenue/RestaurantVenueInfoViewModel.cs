using System;
using System.Collections.Generic;
using System.Text;

namespace AQDiningPortal.Core.Models.RestaurantVenue
{
    public class RestaurantVenueInfoViewModel
    {
        public long Id { get; set; }
        public int VenueFid { get; set; }
        public int LanguageFid { get; set; }
        public string ItemDescription { get; set; }
        public bool UsingSpecialImage { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
