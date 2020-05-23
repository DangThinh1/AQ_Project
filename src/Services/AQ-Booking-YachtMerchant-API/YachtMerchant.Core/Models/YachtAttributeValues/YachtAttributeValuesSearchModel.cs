using AQBooking.Core.Paging;

namespace YachtMerchant.Core.Models.YachtAttributeValues
{
    public class YachtAttributeValuesSearchModel: SearchModel
    {
        public int? AttributeCategoryFid { get; set; }
        public int YachtFid { get; set; }
        public int AttributeFid { get; set; }

        public YachtAttributeValuesSearchModel() : base()
        {
            AttributeCategoryFid = null;
            YachtFid = 0;
            AttributeFid = 0;
        }
    }
}
