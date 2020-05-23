using AQBooking.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharterings
{
    public class YachtCharteringsSearchModel
    {
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtCharteringsSearchPagingModel:SearchModel
    {
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtCharteringsOfMerchantSearchModel
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


    public class YachtCharteringsOfMerchantSearchPagingModel : SearchModel
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

    public class YachtCharteringsOfYachtSearchModel
    {
        public int YachtId { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtCharteringsOfYachtSearchPagingModel:SearchModel
    {
        public int YachtId { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string CharterDate { get; set; }
        public string CustomerName { get; set; }
        public string CharteringCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string YachtPortName { get; set; }
    }

    public class YachtCharteringsDetailSearchPagingModel : SearchModel
    {
        public long ID { get; set; } = 0;

    }
}
