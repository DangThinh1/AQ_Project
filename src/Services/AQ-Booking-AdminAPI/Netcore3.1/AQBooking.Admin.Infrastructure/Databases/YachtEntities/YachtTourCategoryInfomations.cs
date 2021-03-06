﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Databases.YachtEntities
{
    public partial class YachtTourCategoryInfomations
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
        public DateTime EffectiveDate { get; set; }
        public bool Deleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

    }
}
