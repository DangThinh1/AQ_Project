using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.Subscriber
{
    public class SubscriberViewModel
    {
        public string Email { get; set; }
        public string SourceUrl { get; set; }
        public string ModuleName { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
