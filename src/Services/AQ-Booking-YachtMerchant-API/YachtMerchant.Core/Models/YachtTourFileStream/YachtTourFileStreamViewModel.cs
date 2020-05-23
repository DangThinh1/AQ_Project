using System;

namespace YachtMerchant.Core.Models.YachtTourFileStream
{
    public class YachtTourFileStreamViewModel
    {
        public long Id { get; set; }
        public string YachtTourFid { get; set; }
        public int FileCategoryFid { get; set; }
        public string FileCategoryResKey { get; set; }
        public int FileTypeFid { get; set; }
        public string FileTypeResKey { get; set; }
        public long FileStreamFid { get; set; }
        public bool Deleted { get; set; }
        public DateTime ActivatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}