
using System.ComponentModel.DataAnnotations;

namespace AccommodationMerchant.Core.Models.HotelReservations
{
    public class HotelReservationUpdateModel: HotelReservationCreateModel
    {
        [Required]
        public long ReservationsFid { get; set; }
    }
}
