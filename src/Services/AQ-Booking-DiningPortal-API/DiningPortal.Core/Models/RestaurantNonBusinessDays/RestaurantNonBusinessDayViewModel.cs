using System;

namespace AQDiningPortal.Core.Models.RestaurantNonBusinessDays
{
    public class RestaurantNonBusinessDayViewModel
    {
        //public int Id { get; set; }
        //public int RestaurantFid { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remark { get; set; }
    }
}
