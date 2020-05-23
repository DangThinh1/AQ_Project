using AQBooking.YachtPortal.Core.Models.Yachts;
using AQConfigurations.Core.Models.CommonValues;
using AQS.BookingMVC.Infrastructure.AQPagination;
using System.Collections.Generic;

namespace AQS.BookingMVC.Areas.Yacht.Models
{
    public class YachtSearchViewModel
    {
        public YachtSearchViewModel()
        {
            Categories = new List<CommonValueViewModel>();
            CharterTypes = new List<CommonValueViewModel>();
            YachtTypes = new List<CommonValueViewModel>();
            HullTypes = new List<CommonValueViewModel>();
            YatchItems = new PagedListClient<YachtPrivateCharterViewModel>();
            YachtSearchModel = new YachtSearchModel();
        }
      
        public List<CommonValueViewModel> Categories { get; set; }
        public List<CommonValueViewModel> CharterTypes { get; set; }
        public List<CommonValueViewModel> YachtTypes { get; set; }
        public List<CommonValueViewModel> HullTypes { get; set; }
        public PagedListClient<YachtPrivateCharterViewModel> YatchItems { get; set; }
        public YachtSearchModel YachtSearchModel { get; set; }
    }
}
