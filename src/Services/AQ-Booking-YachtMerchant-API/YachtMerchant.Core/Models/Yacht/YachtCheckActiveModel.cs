namespace YachtMerchant.Core.Models.Yacht
{
    public class YachtCheckActiveModel
    {
        public bool CheckInformation { get; set; }
        public bool CheckImage { get; set; }
        public bool CheckRefImage { get; set; }
        public bool CheckPricingPlan { get; set; }
        public bool CheckActiveForOperation { get; set; }
        public bool Allow { get; set; } = false;
    }
}