using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharterings
{
    public class YachtCharteringsConfirmStatusModel
    {
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
