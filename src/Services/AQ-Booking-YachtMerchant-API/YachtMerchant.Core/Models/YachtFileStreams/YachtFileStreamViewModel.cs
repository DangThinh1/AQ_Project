using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtFileStreams
{
    public class YachtFileStreamViewModel
    {
        public int Id { get; set; }
        public int YachtFid { get; set; }
        public int FileTypeFid { get; set; }
        public string FileTypeResKey { get; set; }
        public int FileCategoryFid { get; set; }
        public string FileCategoryResKey { get; set; }
        public int FileStreamFid { get; set; }
        public bool Deleted { get; set; }
        public DateTime ActivatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
