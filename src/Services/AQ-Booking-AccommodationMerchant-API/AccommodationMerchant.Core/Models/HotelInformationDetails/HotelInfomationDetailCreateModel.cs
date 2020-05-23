using System;

namespace AccommodationMerchant.Core.Models.HotelInformationDetails
{
    public class HotelInformationDetailCreateModel
    {
        public int InformationFid { get; set; }
        public int LanguageFid { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}