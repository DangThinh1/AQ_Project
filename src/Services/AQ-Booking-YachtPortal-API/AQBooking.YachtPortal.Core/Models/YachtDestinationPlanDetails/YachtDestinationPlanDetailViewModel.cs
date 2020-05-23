namespace AQBooking.YachtPortal.Core.Models.YachtDestinationPlanDetails
{
    public class YachtDestinationPlanDetailViewModel
    {
        public string Name { get; set; }
        public string Remark { get; set; }
        public bool HaveFileStream { get; set; }
        public int? FileTypeFid { get; set; }
        public int? FileStreamFid { get; set; }
    }
}
