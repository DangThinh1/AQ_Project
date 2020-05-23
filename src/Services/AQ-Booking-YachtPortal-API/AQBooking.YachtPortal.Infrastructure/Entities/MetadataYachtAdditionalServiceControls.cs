using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtAdditionalServiceControls
    {
        public virtual Yachts Yacht { get; set; }
        public virtual YachtAdditionalServices AdditionalService { get; set; }
    }
}
