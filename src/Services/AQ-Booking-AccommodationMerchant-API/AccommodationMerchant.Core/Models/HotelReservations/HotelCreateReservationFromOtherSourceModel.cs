using AccommodationMerchant.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccommodationMerchant.Core.Models.HotelReservations
{
    public class HotelCreateReservationFromOtherSourceModel
    {
        [Required]
        public ReservationSourceEnum source { get; set; }
        public int HotelFid { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string ReservationEmail { get; set; }
        public string ContactNo { get; set; }
        public string CustomerNote { get; set; }
        [Required]
        public int Adult { get; set; }
        [Required]
        public int Child { get; set; }
        [Required]
        public DateTime DiningDate { get; set; }
        [Required]
        public int StatusFid { get; set; }

    }
}
