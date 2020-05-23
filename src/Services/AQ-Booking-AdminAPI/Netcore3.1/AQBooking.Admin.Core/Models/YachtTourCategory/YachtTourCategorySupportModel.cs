using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Core.Models.YachtTourCategory
{
    public class YachtTourCategorySupportModel
    {
        public long Id { get; set; }
        public int LanguageFid { get; set; }
        public string LanguageName { get; set; }
        public string ResourceKey { get; set; }
        public bool Supported { get; set; }
        public int? FileStreamFid { get; set; }
        public int TourCategoryFid { get; set; }
    }
}
