using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharterProcessingFees
{
    public class YachtTourCharterProcessingFeesCreateModel
    {
        [Required]
        public long TourCharterFid { get; set; }
        [Required]
        public double ProcessingValue { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        public double ProcessingFee { get; set; }
        public string Remark { get; set; }
    }
}
