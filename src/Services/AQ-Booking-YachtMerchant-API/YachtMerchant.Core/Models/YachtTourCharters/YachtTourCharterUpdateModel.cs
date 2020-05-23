using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharters
{
    public class YachtTourCharterUpdateModel: YachtTourCharterCreateModel
    {
        [Required]
        public long TourCharterFid { get; set; }
    }
}
