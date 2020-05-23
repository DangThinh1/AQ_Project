using System;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtExternalRefLinks
    {
        public long Id { get; set; }
        public int YachtFid { get; set; }
        public int LinkTypeFid { get; set; }
        public string LinkTypeResKey { get; set; }
        public string Name { get; set; }
        public string UrlLink { get; set; }
        public string Remark { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}