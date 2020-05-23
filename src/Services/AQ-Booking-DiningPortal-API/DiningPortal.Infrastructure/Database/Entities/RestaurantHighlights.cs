using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities
{
    public partial class RestaurantHighlights
    {
        public int Id { get; set; }
        public int RestaurantFid { get; set; }
        public string UniqueId { get; set; }
        public int LanguageFid { get; set; }
        public bool Deleted { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasPrice { get; set; }
        public double? Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
