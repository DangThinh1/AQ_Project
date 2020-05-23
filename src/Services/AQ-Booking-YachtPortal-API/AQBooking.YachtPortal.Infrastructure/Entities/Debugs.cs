using System;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class Debugs
    {
        public int Id { get; set; }
        public string Section { get; set; }
        public string LogMessage { get; set; }
        public DateTime LogDate { get; set; }
    }
}