using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Core.Models.Yachts;
using System.Collections.Generic;

namespace AQS.BookingMVC.Areas.Yacht.Models
{
    public class YachtDetailViewModel
    {
       
        public YachtDetailViewModel()
        {
            YachtPassParamsModel = new YachtPassParamsModel();
        }
      
        public YachtPassParamsModel YachtPassParamsModel { get; set; }
        public List<YachtFileStreamViewModel> ListYatchImage { get; set; }
        public string FullDescriptions { get; set; }
        public string ShortDescriptions { get; set; }
        public YachtAttributeValueCharterPrivateGeneralViewModel Details { get; set; }
        public List<YachtAttributeValueViewModel> Amenities { get; set; }
    }
}
