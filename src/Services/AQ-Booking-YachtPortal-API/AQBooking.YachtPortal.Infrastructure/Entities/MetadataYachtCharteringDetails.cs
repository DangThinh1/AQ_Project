using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtCharteringDetails
    {
        public virtual YachtCharterings Chartering { get; set; }
    }
}
