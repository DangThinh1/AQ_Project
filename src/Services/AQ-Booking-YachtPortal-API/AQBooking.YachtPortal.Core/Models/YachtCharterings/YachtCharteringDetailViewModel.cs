﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtCharterings
{
    public class YachtCharteringDetailViewModel
    {
        public long Id { get; set; }
        public int YachtFid { get; set; }
        public long CharteringFid { get; set; }
        public int ItemTypeFid { get; set; }
        public string ItemTypeResKey { get; set; }
        public int RefFid { get; set; }
        public string ItemName { get; set; }
        public string CurrencyCode { get; set; }
        public string CultureCode { get; set; }
        public double OriginalValue { get; set; }
        public double DiscountedValue { get; set; }
        public double FinalValue { get; set; }
        public int OrderAmount { get; set; }
        public double GrandTotalValue { get; set; }
        public string Remark { get; set; }
    }
}