using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTours
{
    public class YachtTourCheckActiveModel
    {
        public bool CheckTourInfo { get; set; }
        public bool CheckTourImage { get; set; }
        public bool CheckTourRefImage { get; set; }
        public bool CheckTourPricing { get; set; }
        public bool CheckTourOperationDetail { get; set; }
        public bool CheckActiveTour { get; set; }
        public bool Allow { get; set; } = false;
    }
}
