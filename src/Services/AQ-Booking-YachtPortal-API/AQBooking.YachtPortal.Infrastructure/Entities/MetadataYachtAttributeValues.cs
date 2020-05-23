using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtAttributeValues
    {
        public virtual Yachts Yacht { get; set; }
        public virtual YachtAttributes Attribute { get; set; }
    }
}
