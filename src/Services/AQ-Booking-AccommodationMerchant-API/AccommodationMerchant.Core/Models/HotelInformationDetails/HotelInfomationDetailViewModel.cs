using System;

namespace AccommodationMerchant.Core.Models.HotelInformationDetails
{
    public class HotelInformationDetailViewModel
    {
        //Decrypt values
        public string Id { get; set; }
        public string InformationFid { get; set; }

        //Normal values
        public string UniqueId { get; set; }
        public int LanguageFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
        public bool IsActivated { get; set; }
        public string ActivatedDate { get; set; }
    }
}