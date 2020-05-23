using System;

namespace YachtMerchant.Core.Models.YachtTourCategory
{
    public class YachtTourCategoryViewModel
    {
        public int Id { get; set; }
        public string DefaultName { get; set; }
        public string ResourceKey { get; set; }
        public string Remark { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public bool IsActivated { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
        public double? OrderBy { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
