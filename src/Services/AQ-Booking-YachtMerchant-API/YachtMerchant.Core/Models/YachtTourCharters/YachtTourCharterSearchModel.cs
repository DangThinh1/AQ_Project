using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharters
{
    public class YachtTourCharterSearchModel
    {
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtTourCharterSearchPagingModel:SearchModel
    {
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtTourCharterOfMerchantSearchModel
    {
        public int MerchantId { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }


    public class YachtTourCharterOfMerchantSearchPagingModel : SearchModel
    {
        public int MerchantId { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtTourCharterOfTourSearchModel
    {
        public int YachtTourFid { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtTourCharterOfTourSearchPagingModel:SearchModel
    {
        public int YachtTourFid { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtTourCharterDetailSearchPagingModel : SearchModel
    {
        public long ID { get; set; } = 0;

    }
}
