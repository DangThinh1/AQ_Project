using YachtMerchant.Core.DTO;
using System.Collections.Generic;

namespace YachtMerchant.Core.Models.YachtMerchant
{
    public class YachtMerchantModel
    {
        public string RoleId { get; set; }
        public string MerchantId { get; set; } = "";
        public string YachtId { get; set; } = "";
        public string MerchantIdDefault { get; set; } = "";
        public string YachtIdDefault { get; set; } = "";
        public List<Merchant> MerchantsList { get; set; }
        public List<DTODropdownItem> YachtList { get; set; }

    }
}
