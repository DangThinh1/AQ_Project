using System.Collections.Generic;
namespace AQBooking.YachtPortal.Core.Models.YachtPricingPlanInfomation
{
    public class YachtPricingPlanInfomationViewModel
    {
        public long Id { get; set; }
        public int PricingPlanFid { get; set; }
        public int LanguageFid { get; set; }
        public string PackageInfo { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
    }
}
