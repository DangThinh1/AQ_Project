using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.Yacht
{
    public class YachtBasicProfileModel
    {
        public int YachtId { get; set; }
        public string YachtUniqueId { get; set; }
        public int MerchantId { get; set; }
        public string YachtName { get; set; }

    }
}
