using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharterSchedules
{
    public class GetAllSchedulesSetOnReservationModel
    {
        public long TourCharterFid { get; set; }
        public int YachtId { get; set; }
    }
}
