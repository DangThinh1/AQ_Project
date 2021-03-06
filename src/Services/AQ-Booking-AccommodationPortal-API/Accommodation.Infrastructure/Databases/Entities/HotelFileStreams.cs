﻿using System;
using System.Collections.Generic;

namespace Accommodation.Infrastructure.Databases.Entities
{
    public partial class HotelFileStreams
    {
        public long Id { get; set; }
        public int HotelFid { get; set; }
        public int FileCategoryFid { get; set; }
        public string FileCategoryResKey { get; set; }
        public int FileTypeFid { get; set; }
        public string FileTypeResKey { get; set; }
        public int FileStreamFid { get; set; }
        public bool Deleted { get; set; }
        public DateTime ActivatedDate { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}