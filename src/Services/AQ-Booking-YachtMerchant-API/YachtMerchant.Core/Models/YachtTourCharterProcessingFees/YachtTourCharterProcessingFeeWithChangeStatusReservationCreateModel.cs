using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharterProcessingFees
{
    public class YachtTourCharterProcessingFeeWithChangeStatusReservationCreateModel
    {
        // YachtTourCharterProcessingFees
        [Required]
        public long TourCharterFid { get; set; }
        [Required]
        public double ProcessingValue { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        public double ProcessingFee { get; set; }
        public string Remark { get; set; }

        //YachtTourCharters
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
