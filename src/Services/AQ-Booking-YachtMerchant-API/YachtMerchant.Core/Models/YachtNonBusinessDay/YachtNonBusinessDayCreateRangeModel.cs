using System.ComponentModel.DataAnnotations;

namespace YachtMerchant.Core.Models.YachtNonBusinessDay
{
    public class YachtNonBusinessDayCreateRangeModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "The YachtFid field is required.")]
        public int YachtFid { get; set; }
        public string NonBusinessDay { get; set; }
        public bool Recurring { get; set; }
        public string Remark { get; set; }
    }
}
