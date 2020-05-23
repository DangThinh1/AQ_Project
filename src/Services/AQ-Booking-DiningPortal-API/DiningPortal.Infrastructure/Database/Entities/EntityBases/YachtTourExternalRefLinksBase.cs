using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtTourExternalRefLinksBase
    {
        public long Id { get; set; }
        public int YachtTourFid { get; set; }
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