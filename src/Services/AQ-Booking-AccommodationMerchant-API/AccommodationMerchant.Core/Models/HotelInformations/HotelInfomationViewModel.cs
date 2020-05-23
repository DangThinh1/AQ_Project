using System;
using System.Collections.Generic;

namespace AccommodationMerchant.Core.Models.HotelInformations
{
    public class HotelInformationViewModel
    {
        public int Id { get; set; }
        public string DefaultTitle { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public List<string> LanguagesSupported { get; set; }
    }
}