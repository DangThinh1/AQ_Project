using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharterSchedules
{
    public class YachtTourCharterSchedulesUpdateModel: YachtTourCharterSchedulesCreateModel
    {
        [Required]
        public long Id { get; set; }
    }
}
