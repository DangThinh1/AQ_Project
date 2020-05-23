using System.ComponentModel.DataAnnotations;

namespace AccommodationMerchant.Core.Models.HotelReservations
{
    public class HotelReservationConfirmStatusModel
    {
        public string Id { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Stage { get; set; } // "true", "false"
        public string Status { get; set; }  //-->Status Fid
        public string ProcessRemark { get; set; }
        public string CancelRemark { get; set; }
    }
}
