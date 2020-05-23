using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtMerchantFileStream
{
    public class YachtMerchantFileStreamUpdateModel : YachtMerchantFileStreamCreateModel
    {
        public long Id { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
