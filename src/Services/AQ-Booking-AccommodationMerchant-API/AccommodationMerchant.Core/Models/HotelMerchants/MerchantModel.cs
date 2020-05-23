using System.Collections.Generic;

namespace AccommodationMerchant.Core.Models.HotelMerchants
{
    public class MerchantModel
    {
        public string MerchantId { get; set; } = "";
        public string MerchantIdDefault { get; set; } = "";
        public List<Merchant> MerchantsList { get; set; }
    }
}
