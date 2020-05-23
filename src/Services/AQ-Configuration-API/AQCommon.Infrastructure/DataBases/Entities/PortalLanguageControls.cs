using System;
using System.Collections.Generic;

namespace AQConfigurations.Infrastructure.Databases.Entities
{
    public partial class PortalLanguageControls
    {
        public int Id { get; set; }
        public string PortalUniqueId { get; set; }
        public int DomainPortalFid { get; set; }
        public int LanguageFid { get; set; }
        public bool Deleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual CommonLanguages Language { get; set; }
    }
}
