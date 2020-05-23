using System.Collections.Generic;

namespace YachtMerchant.Core.Models.YachtMerchant
{
    public class MerchantModel
    {
        public string MerchantId { get; set; } = "";
        public string MerchantIdDefault { get; set; } = "";
        public List<Merchant> MerchantsList { get; set; }
    }
}
