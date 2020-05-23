using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class PortLocations
    {
        public virtual List<YachtPorts> YachtPorts { get; set; }

        public PortLocations()
        {
            YachtPorts = new List<YachtPorts>();
        }
    }
}
