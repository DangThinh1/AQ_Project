using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourInformations
{
    public class YachtTourInformationViewModel
    {
        public long Id { get; set; }
        public int TourFid { get; set; }
        public List<string> LanguagesSupported { get; set; }
        public bool IsActivated { get; set; }
        public int TourInformationTypeFid { get; set; }
        public string TourInformationTypeResKey { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}
