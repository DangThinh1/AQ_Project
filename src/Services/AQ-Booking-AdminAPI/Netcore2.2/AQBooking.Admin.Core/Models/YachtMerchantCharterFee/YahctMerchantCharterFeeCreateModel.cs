using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtMerchantCharterFee
{
    public class YahctMerchantCharterFeeCreateModel
    {
        public int MerchantFid { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Remark { get; set; }
        public int BookingFeeOptionFid { get; set; }
        public string BookingFeeOptionResKey { get; set; }
        public bool IsSchemeBased { get; set; }
        public int SchemeBasedOptionFid { get; set; }
        public string SchemeBasedOptionResKey { get; set; }
        public double Level { get; set; }
        public double BookingFeePercent { get; set; }
        public double MinTarget { get; set; }
        public double MaxTarget { get; set; }
    }
}
