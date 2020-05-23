using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationMerchant.Core.Models.HotelFileStreamModel
{
    public class HotelFileStreamSearchModel : PagableModel
    {
        public int FileCategoryFid { get; set; }
        public int FileTypeFid { get; set; }
        public DateTime ActivatedDate { get; set; }
    }
}
