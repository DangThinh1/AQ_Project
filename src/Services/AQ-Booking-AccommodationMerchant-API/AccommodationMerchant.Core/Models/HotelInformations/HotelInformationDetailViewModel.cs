using System;

namespace AccommodationMerchant.Core.Models.HotelInformations
{
    public class HotelInformationDetailViewModel
    {
        public int Id { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }
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