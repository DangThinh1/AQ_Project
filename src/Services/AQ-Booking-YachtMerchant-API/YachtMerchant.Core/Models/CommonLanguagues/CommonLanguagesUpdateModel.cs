using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.CommonLanguagues
{
    public class CommonLanguagesUpdateModel : CommonLanguagesRequestModel
    {
        [Required]
        public int Id { get; set; }
    }
}
