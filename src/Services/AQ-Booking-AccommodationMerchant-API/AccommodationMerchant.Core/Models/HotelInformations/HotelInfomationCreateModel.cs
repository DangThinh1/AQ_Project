using System;

namespace AccommodationMerchant.Core.Models.HotelInformations
{
    public class HotelInformationCreateModel
    {
        //Security Foreign Key
        public string HotelFid { get; set; }

        //HotelInformation values
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }

        //HotelInformationDetails values
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