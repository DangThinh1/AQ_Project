using System;

namespace YachtMerchant.Core.Models.YachtTourFileStream
{
    public class YachtTourFileStreamCreateModel
    {
        public int FileTypeFid { get; set; }
        public string FileTypeResKey { get; set; }
        public long FileStreamFid { get; set; }
        public int FileCategoryFid { get; set; }
        public string FileCategoryResKey { get; set; }
        public DateTime ActivatedDate { get; set; }
    }
}