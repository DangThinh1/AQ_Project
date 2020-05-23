using System;

namespace YachtMerchant.Core.Models.YachtNonBusinessDay
{
    public class YachtNonBusinessDayViewModel
    {
        public int Id { get; set; }
        public int YachtFid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Recurring { get; set; }
        public string Remark { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}