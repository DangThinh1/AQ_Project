using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtMerchantCharterFee
{
    public class YachtMerchantCharterFeeUpdateModel : YahctMerchantCharterFeeCreateModel
    {
        public long Id { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
