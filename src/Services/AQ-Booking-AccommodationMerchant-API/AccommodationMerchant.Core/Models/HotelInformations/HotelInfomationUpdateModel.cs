namespace AccommodationMerchant.Core.Models.HotelInformations
{
    public class HotelInformationUpdateModel
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public bool IsActivated { get; set; }
        public string ActivatedDate { get; set; }

        //public int HotelFid { get; set; }
    }
}