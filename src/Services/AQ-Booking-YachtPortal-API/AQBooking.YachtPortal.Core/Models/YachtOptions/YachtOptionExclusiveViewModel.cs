
namespace AQBooking.YachtPortal.Core.Models.YachtOptions
{
    public class YachtOptionExclusiveViewModel
    {
        public string Id { get; set; }
        public string MerchantFid { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public int LandingPageOptionFid { get; set; }
        public string MerchantName { get; set; }
        public int? YachtFileStreamId { get; set; }
        public int? MerchantFileStreamId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string YachtTypeResKey { get; set; }
        public long? CharteringFId { get; set; }
    }
}
