using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using YachtMerchant.Core.Enum;

namespace YachtMerchant.Core.Models.YachtCharterings
{
    public class CreateCharteringFromOtherSourceModel
    {
        [Required]
        public YachtCharteringSourceEnum source { get; set; }
        public int YachtFid { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string ReservationEmail { get; set; }
        [Required]
        public int Passengers { get; set; }
        [Required]
        public DateTime CharterDateFrom { get; set; }
        [Required]
        public DateTime CharterDateTo { get; set; }
        [Required]
        public int YachtPortFid { get; set; }
        public string YachtPortName { get; set; }
        [Required]
        public bool HaveAdditionalServices { get; set; }
        [Required]
        public bool HaveCrewsMember { get; set; }
        [Required]
        public bool HaveChef { get; set; }
        [Required]
        public int PricingTypeFid { get; set; }
        [Required]
        public string PricingTypeResKey { get; set; }
      
    }
}
