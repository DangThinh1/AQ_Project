namespace AQBooking.YachtPortal.Core.Models.YachtAttributeValues
{
    public class YachtAttributeValueViewModel
    {
        public int AttributeFid { get; set; }
        public int? AttributeCategoryFid { get; set; }
        public string AttributeName { get; set; }

        public string IconCssClass { get; set; }
        public string ResourceKey { get; set; }
        public string Remarks { get; set; }
        public string AttributeValue { get; set; }
    }
}
