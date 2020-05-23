using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.PortLocation
{
    public class PortLocationUpdateModel : PortLocationCreateModel
    {
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
