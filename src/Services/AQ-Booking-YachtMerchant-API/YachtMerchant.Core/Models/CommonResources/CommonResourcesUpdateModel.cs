using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.CommonResources
{
    public class CommonResourcesUpdateModel : CommonResourcesRequestModel
    {
        [Required]
        public int ResourceId { get; set; }
    }
}
