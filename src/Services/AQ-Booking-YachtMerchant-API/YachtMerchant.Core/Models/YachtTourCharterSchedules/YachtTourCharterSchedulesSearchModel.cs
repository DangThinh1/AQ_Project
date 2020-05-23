using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharterSchedules
{
    public class YachtTourCharterSchedulesSearchModel
    {
        public string UserCode { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public int Role { get; set; }
    }
    public class YachtTourCharterSchedulesSearchpagingModel:SearchModel
    {
        public string UserCode { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public int Role { get; set; }
    }
}
