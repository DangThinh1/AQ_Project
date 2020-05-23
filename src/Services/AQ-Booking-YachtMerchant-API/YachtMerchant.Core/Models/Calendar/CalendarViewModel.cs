using System;
using System.Collections.Generic;

namespace YachtMerchant.Core.Models.Calendar
{

    public class YachtCalendar
    {
        public long Id { get; set; }
        public int YachtId { get; set; }
        public string YachtName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Status { get; set; }
        public bool Recurring { get; set; }
    }
}
