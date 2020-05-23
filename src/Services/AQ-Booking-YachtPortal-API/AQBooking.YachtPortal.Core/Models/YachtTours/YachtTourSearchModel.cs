using AQBooking.YachtPortal.Core.Models.Common;
using System;

namespace AQBooking.YachtPortal.Core.Models.YachtTours
{
    public class YachtTourSearchModel : PagableModel
    {
        public int LocationId { get; set; }
        public string CityName { get; set; }
        public int CityFid { get; set; }
        public int TourCategoryFid { get; set; }
        public DateTime DepartTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public int Passenger { get; set; }

        public YachtTourSearchModel()
        {
            PageIndex = 1;
            PageSize = 12;
            Passenger = 2;
        }
    }
}
