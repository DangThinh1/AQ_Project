using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtPorts
{
    public class YachtPortViewModel
    {
        public long Id { get; set; }
        public int YachtFid { get; set; }
        public int PortFid { get; set; }
        public string PortName { get; set; }
    }
}
