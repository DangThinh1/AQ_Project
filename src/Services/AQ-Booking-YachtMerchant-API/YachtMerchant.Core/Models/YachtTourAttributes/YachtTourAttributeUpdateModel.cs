namespace YachtMerchant.Core.Models.YachtTourAttributes
{
    public class YachtTourAttributeUpdateModel
    {
        public int? AttributeCategoryFid { get; set; }
        public string AttributeName { get; set; }
        public string IconCssClass { get; set; }
        public string ResourceKey { get; set; }
        public string Remarks { get; set; }
        public bool IsDefault { get; set; }
        public double? OrderBy { get; set; }
    }
}