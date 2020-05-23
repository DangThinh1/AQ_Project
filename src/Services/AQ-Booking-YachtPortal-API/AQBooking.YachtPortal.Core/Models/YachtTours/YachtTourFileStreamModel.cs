using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtTours
{
    public class YachtTourFileStreamModel
    {
        public int FileCategoryFid { get; set; }
        public string FileCategoryResKey { get; set; }
        public int FileTypeFid { get; set; }
        public long FileStreamFid {get; set; }
    }
}
