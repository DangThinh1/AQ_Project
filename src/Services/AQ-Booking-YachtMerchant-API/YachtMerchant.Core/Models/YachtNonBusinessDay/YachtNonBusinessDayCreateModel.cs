using System;
using System.ComponentModel.DataAnnotations;

namespace YachtMerchant.Core.Models.YachtNonBusinessDay
{
    public class YachtNonBusinessDayCreateModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "The YachtFid field is required.")]
        public int YachtFid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Recurring { get; set; }
        public string Remark { get; set; }
    }
}
