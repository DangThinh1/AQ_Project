using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AccommodationMerchant.Core.Models.HotelMerchants
{
    public class HotelMerchantModel
    {
        public string RoleId { get; set; }
        public string MerchantId { get; set; } = "";
        public string HotelFid { get; set; } = "";
        public string MerchantIdDefault { get; set; } = "";
        public string HotelFidDefault { get; set; } = "";
        public List<Merchant> MerchantsList { get; set; }
        public List<SelectListItem> HotelList { get; set; }

    }
}
