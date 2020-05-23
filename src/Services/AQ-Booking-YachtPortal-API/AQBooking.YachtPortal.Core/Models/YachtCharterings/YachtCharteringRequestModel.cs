using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtCharterings
{
    public class YachtCharteringRequestModel
    {
        public string YachtFId { get; set; }
        public List<int> StatusId { get; set; }
        public string CheckOut { get; set; }
        public string CheckIn { get; set; }
    }
}
