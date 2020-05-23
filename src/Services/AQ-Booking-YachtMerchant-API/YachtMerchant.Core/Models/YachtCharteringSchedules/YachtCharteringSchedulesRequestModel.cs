using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharteringSchedules
{
    public class YachtCharteringSchedulesRequestModel
    {
        [Required]
        public long Id { get; set; }
    }
}
