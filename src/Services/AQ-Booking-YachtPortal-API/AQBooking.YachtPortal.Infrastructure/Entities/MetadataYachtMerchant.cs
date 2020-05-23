using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class YachtMerchants
    {
        public virtual List<Yachts> Yachts { get; set; }
        public virtual List<YachtMerchantInformations> Informations { get; set; }
        public virtual List<YachtMerchantFileStreams> FileStreams { get; set; }

        public YachtMerchants()
        {
            Yachts = new List<Yachts>();
            Informations = new List<YachtMerchantInformations>();
            FileStreams = new List<YachtMerchantFileStreams>();
        }
    }
}
