namespace AccommodationMerchant.Core.Models.Hotels
{
    public class HotelSearchResultModel
    {
        //Caculated values
        //Origin values
        public string Id { get; set; }
        public string MerchantFid { get; set; }
        public string HotelName { get; set; }
        public string FullAddress { get; set; }
        public int TotalRooms { get; set; }
        public string HotelTypeResKey { get; set; }
        public int HotelTypeFid { get; set; }
        public int HotelCategoryFid { get; set; }
        public string HotelCategoryResKey { get; set; }
        public bool ActiveForOperation { get; set; }
    }
}