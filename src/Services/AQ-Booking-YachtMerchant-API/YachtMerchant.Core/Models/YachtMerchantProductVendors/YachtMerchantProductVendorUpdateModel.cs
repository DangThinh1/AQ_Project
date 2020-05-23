using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtMerchantProductVendors
{
   public class YachtMerchantProductVendorUpdateModel : YachtMerchantProductVendorCreateModel
    {
        [Required]
        public int Id { get; set; }
    }
}
