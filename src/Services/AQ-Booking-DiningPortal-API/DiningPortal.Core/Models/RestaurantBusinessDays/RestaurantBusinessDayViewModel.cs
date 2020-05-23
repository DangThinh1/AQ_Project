using AQDiningPortal.Core.Models.RestaurantBusinessDayOperations;

namespace AQDiningPortal.Core.Models.RestaurantBusinessDays
{
    public class RestaurantBusinessDayViewModel
    {
        public int BusinessDay { get; set; }
        public string DayOfWeek { get; set; }

        public RestaurantBusinessDayOperationModel BusinessDayOperation { get; set; }
    }
}
