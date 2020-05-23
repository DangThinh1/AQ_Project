using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharteringProcessingFees
{
    public class YachtCharteringProcessingFeesViewModel
    {
        public long CharteringId { get; set; }
        public double ProcessingValue { get; set; }
        public double Percentage { get; set; }
        public double ProcessingFee { get; set; }
        public string Remark { get; set; }
        public DateTime GeneratedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
