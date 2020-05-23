using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.PortalLocation
{
    public class PortalLocationViewModel : PortalLocationCreateModel
    {
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
