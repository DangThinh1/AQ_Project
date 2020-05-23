using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtMerchantInformations
    {
        public virtual YachtMerchants Merchant { get; set; }
        public virtual List<YachtMerchantInformationDetails> InformationDetails { get; set; }

        public YachtMerchantInformations()
        {
            InformationDetails = new List<YachtMerchantInformationDetails>();
        }
    }
}
