using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Database.Entities.EntityBases
{
    public partial class YachtPortsBase
    {
        public long Id { get; set; }
        public int YachtFid { get; set; }
        public int PortFid { get; set; }
        public string PortName { get; set; }
        public int PortTypeFid { get; set; }
        public string PortTypeResKey { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remark { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}