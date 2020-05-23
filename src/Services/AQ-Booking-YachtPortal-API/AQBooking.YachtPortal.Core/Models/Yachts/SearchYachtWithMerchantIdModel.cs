using AQBooking.YachtPortal.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.Yachts
{
    public class SearchYachtWithMerchantIdModel: PagableModel
    {
        public string MerchantId { get; set; }
    }
}
