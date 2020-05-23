using AQBooking.YachtPortal.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class YachtbookingRequestModel: PagableModel
    {
        public List<int> statusId { get; set; }
        public string CustomerId { get; set; }
        public int YachtPortId { get; set; }
        public string ExtendData { get; set; }
    }
}
