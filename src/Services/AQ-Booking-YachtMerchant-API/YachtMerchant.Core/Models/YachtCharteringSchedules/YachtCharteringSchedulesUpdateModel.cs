using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharteringSchedules
{
    public class YachtCharteringSchedulesUpdateModel: YachtCharteringSchedulesCreateModel
    {
        [Required]
        public long Id { get; set; }
    }
}
