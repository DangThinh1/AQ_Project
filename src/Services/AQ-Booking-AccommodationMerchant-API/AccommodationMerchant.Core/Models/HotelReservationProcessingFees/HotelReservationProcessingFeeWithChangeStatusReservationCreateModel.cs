using System.ComponentModel.DataAnnotations;

namespace AccommodationMerchant.Core.Models.HotelReservationProcessingFees
{ 
    public class HotelReservationProcessingFeeWithChangeStatusReservationCreateModel
    {
        // RestaurantDiningReservationProcessingFees 
        [Required]
        public long ReservationsFid { get; set; }
        [Required]
        public double ProcessingValue { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        public double ProcessingFee { get; set; }
        public string Remark { get; set; }

        //RestaurantDiningReservations
        [Required]
        public string Id { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false), Required]
        public string Stage { get; set; } // "true", "false"
        [Required]
        public string Status { get; set; }  //-->Status Fid 
        public string ProcessRemark { get; set; }
        public string CancelRemark { get; set; }
    }
}
