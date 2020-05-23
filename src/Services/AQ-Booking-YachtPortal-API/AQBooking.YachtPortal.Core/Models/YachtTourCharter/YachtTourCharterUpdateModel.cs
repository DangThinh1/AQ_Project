using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtTourCharter
{
    public class YachtTourCharterUpdateModel
    {
        public int CharterId { get; set; }
        public string CharterUniqueId { get; set; }
        public string CreatedUser { get; set; }
        public string TransactionId { get; set; }
        public int StatusId { get; set; }
    }
}
