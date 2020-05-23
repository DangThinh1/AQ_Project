using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Entities
{
    public partial class YachtInformations
    {
        public int Id { get; set; }
        public int YachtFid { get; set; }
        public string UniqueId { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public Guid? ActivatedBy { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual Yachts Yacht { get; set; }
    }
}