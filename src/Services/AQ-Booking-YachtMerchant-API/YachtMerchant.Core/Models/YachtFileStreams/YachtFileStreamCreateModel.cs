using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtFileStreams
{
    public class YachtFileStreamCreateModel
    {
        public int YachtFid { get; set; }
        public int FileTypeFid { get; set; }
        public string FileTypeResKey { get; set; }
        public int FileStreamFid { get; set; }
        public int FileCategoryFid { get; set; }
        public string FileCategoryResKey { get; set; }
        public DateTime ActivatedDate { get; set; }
    }
}
