using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharteringSchedules
{
    public class GetAllSchedulesSetOnReservationModel
    {
        public long CharteringId { get; set; }
        public int YachtId { get; set; }
    }
}
