using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtInformation
{
    public class YachtInformationsUpdateModel
    {
        [Required]
        public int Id { get; set; }
    }
}
