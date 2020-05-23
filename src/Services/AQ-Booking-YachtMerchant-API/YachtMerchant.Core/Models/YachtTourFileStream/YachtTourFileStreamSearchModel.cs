using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourFileStream
{
    public class YachtTourFileStreamSearchModel: SearchModel
    {
        public string YachtTourEncryptedId { get; set; }
    }
}
