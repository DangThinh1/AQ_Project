using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourNonOperationDays
{
    public class YachtTourNonOperationDaySearchModel: SearchModel
    {
        public string yachtTourIdEncrypted { get; set; }
    }
}
