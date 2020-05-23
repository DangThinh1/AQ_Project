using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtFileStreams
{
    public class YachtFileStreamSearchModel : SearchModel
    {
        public int YachtId { get; set; }
        public int FileTypeFid { get; set; }
        public int FileCategoryFid { get; set; }
    }
}
