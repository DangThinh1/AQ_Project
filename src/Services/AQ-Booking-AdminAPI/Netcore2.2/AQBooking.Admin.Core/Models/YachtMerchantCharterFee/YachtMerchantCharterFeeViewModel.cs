using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtMerchantCharterFee
{
    public class YachtMerchantCharterFeeViewModel : YachtMerchantCharterFeeUpdateModel
    {
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
