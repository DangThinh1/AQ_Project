using AQDiningPortal.Core.Models.RestaurantInformationDetails;
using System.Collections.Generic;

namespace AQDiningPortal.Core.Models.RestaurantInformations
{
    public class RestaurantInformationViewModel
    {
        //public int Id { get; set; }
        //public string UniqueId { get; set; }
        public string DefaultTitle { get; set; }
        public string Remark { get; set; }

        public RestaurantInformationDetailViewModel InformationDetail { get; set; }
    }
}
