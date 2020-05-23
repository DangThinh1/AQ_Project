using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtTourCategory
{
    public class YachtTourCategoryCreateDetailModel
    {
        public long Id { get; set; }
        public int TourCategoryFid { get; set; }
        public string TourCategoryResourceKey { get; set; }
        public int LanguageFid { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public bool UsingSpecialImage { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
    }
}
