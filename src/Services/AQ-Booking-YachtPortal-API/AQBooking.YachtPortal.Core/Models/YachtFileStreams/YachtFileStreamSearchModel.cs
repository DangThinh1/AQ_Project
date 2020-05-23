using AQBooking.YachtPortal.Core.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Core.Models.YachtFileStreams
{
    public class YachtFileStreamSearchModel: PagableModel
    {
        public string YachtFId { get; set; }
        public int FileTypeFId { get; set; }
    }
}
