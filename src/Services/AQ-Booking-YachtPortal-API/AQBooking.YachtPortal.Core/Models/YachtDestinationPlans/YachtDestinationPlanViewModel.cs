using System;

namespace AQBooking.YachtPortal.Core.Models.YachtDestinationPlans
{
    public class YachtDestinationPlanViewModel
    {
        public string DefaultDestinationName { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }

        public string Name { get; set; }
        public string Remark { get; set; }
        public bool HaveFileStream { get; set; }
        public int? FileTypeFid { get; set; }
        public int? FileStreamFid { get; set; }
    }
}
