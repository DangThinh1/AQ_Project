using System;
namespace YachtMerchant.Core.Models.YachtTourNonOperationDays
{
    public class YachtTourNonOperationDayViewModel
    {
        public int Id { get; set; }
        public string YachtTourFid { get; set; }
        public string YachtFid { get; set; }
        public string YachtName { get; set; }
        public bool Deleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Recurring { get; set; }
        public string Remark { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
