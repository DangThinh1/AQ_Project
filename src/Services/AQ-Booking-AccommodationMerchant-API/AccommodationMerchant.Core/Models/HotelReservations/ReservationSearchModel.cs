using AQBooking.Core.Paging;


namespace AccommodationMerchant.Core.Models.HotelReservations
{
    public class ReservationSearchModel
    {
        public int Type { get; set; } = 0;
        public string DiningDate { get; set; }
        public string ReservationCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo{ get; set; }
        
    }

    public class ReservationSearchPagingModel : SearchModel
    {
        public int Type { get; set; } = 0;
        public string DiningDate { get; set; } 
        public string ReservationCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }

    }
    public class ReservationOfHotelSearchModel 
    {
        public int HotelFid { get; set; }
        public int Type { get; set; } = 0;
        public string DiningDate { get; set; }
        public string ReservationCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }

    }
    public class ReservationOfHotelSearchPagingModel : SearchModel
    {
        public int HotelFid { get; set; }
        public int Type { get; set; } = 0;
        public string DiningDate { get; set; }
        public string ReservationCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }

    }

    public class ReservationOfMerchantSearchModel
    {
        public int MerchantFid { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string DiningDate { get; set; }
        public string ReservationCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }

    }

    public class ReservationForMerchantSearchPagingModel:SearchModel
    {
        public int MerchantFid { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string DiningDate { get; set; }
        public string ReservationCode { get; set; }
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
    }

   

    public class ReservationDetailSearchPagingModel : SearchModel
    {
        public long ReservationFid { get; set; } = 0;

    }
}
