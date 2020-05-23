using System;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Entities
{ 
    public partial class YachtNonOperationDays
    {
        public int Id { get; set; }
        public int YachtFid { get; set; }
        public bool Deleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Recurring { get; set; }
        public string Remark { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual Yachts Yacht { get; set; }
    }
}