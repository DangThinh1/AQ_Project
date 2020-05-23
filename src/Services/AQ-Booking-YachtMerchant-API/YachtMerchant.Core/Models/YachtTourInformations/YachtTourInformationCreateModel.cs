using System;

namespace YachtMerchant.Core.Models.YachtTourInformations
{
    public class YachtTourInformationCreateModel
    {
        public int TourFid { get; set; }
        public int LanguageFid { get; set; }
        public int TourInformationTypeFid { get; set; }
        public string TourInformationTypeResKey { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public string Title { get; set; }
        public string ShortDescriptions { get; set; }
        public string FullDescriptions { get; set; }
    }
}