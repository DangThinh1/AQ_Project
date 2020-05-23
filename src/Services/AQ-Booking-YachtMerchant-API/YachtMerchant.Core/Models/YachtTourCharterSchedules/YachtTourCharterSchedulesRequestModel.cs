using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharterSchedules
{
    public class YachtTourCharterSchedulesRequestModel
    {
        [Required]
        public long Id { get; set; }
    }
}
