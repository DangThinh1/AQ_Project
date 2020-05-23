using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YachtMerchant.Core.Enum;

namespace YachtMerchant.Core.Models.YachtTourCharters
{
    public class CreateTourCharterFromOtherSourceModel
    {
        [Required]
        public YachtCharteringSourceEnum source { get; set; }
        public int YachtTourFID { get; set; }
        public int YachtFid { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string ReservationEmail { get; set; }
        [Required]
        public int Passengers { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        [Required]
        public int YachtPortFid { get; set; }
        public string YachtPortName { get; set; }
        
      
    }
}
