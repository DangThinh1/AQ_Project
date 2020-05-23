using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Entities
{
    public partial class YachtDestinationPlanDetails
    {
        public long Id { get; set; }
        public int DestinationPlanFid { get; set; }
        public int LanguageFid { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool HaveFileStream { get; set; }
        public int? FileTypeFid { get; set; }
        public int? FileStreamFid { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}