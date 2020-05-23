using System;

namespace YachtMerchant.Core.Models.YachtTourNonOperationDays
{
    public class YachtTourNonOperationDayCreateModel
    {
        public string YachtTourFid { get; set; }
        public string YachtFid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Recurring { get; set; }
        public string Remark { get; set; }
    }
}