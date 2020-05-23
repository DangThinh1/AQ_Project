using System.ComponentModel.DataAnnotations;

namespace AccommodationMerchant.Core.Models.HotelReservationProcessingFees
{
    public class HotelReservationProcessingFeeCreateModel
    {
        [Required]
        public long ReservationsFid { get; set; }
        [Required]
        public double ProcessingValue { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        public double ProcessingFee { get; set; }
        public string Remark { get; set; }
    }
}
