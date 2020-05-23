using AQDiningPortal.Core.Models.RestaurantAttributeValues;
using AQDiningPortal.Core.Models.RestaurantBusinessDays;
using AQDiningPortal.Core.Models.RestaurantCounters;
using AQDiningPortal.Core.Models.RestaurantFileStreams;
using AQDiningPortal.Core.Models.RestaurantInformations;
using AQDiningPortal.Core.Models.RestaurantMenus;
using AQDiningPortal.Core.Models.RestaurantNonBusinessDays;
using AQDiningPortal.Core.Models.RestaurantOtherInformations;
using AQDiningPortal.Core.Models.RestaurantReservationFees;
using System.Collections.Generic;

namespace AQDiningPortal.Core.Models.Restaurants
{
    public class RestaurantDetailViewModel
    {
        public RestaurantViewModel Restaurant { get; set; }

        public List<RestaurantAttributeValueViewModel> AttributeValues { get; set; }

        public RestaurantInformationViewModel Information { get; set; }
        
        public RestaurantOtherInformationViewModel OtherInformation { get; set; }

        public List<RestaurantBusinessDayViewModel> BusinessDays { get; set; }

        public List<RestaurantNonBusinessDayViewModel> NonBusinessDays { get; set; }

        public List<RestaurantFileStreamViewModel> FileStreams { get; set; }

        public RestaurantCounterViewModel Counter { get; set; }

        public List<RestaurantMenuViewModel> Menus { get; set; }

        public RestaurantReservationFeeViewModel ReservationFee { get; set; }
    }
}
