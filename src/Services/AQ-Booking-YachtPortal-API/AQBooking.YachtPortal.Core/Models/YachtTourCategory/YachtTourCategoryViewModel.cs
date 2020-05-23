using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtTourCategory
{
    public class YachtTourCategoryViewModel
    {
        public string DefaultName { get; set; }
        public string TourCategoryResourceKey { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public int FileTypeFid { get; set; }
        public int FileStreamFid { get; set; }
    }
}
