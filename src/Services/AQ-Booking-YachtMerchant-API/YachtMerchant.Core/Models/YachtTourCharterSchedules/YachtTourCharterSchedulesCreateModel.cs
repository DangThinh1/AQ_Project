using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharterSchedules
{
    public class YachtTourCharterSchedulesCreateModel
    {
        [Required]
        public long TourCharterFid { get; set; }
        [Required]
        public int YachtFid { get; set; }
        [Required]
        public Guid UserFid { get; set; }
        [Required]
        public int RoleFid { get; set; }
        public string RoleResKey { get; set; }
        [Required]
        public DateTime EffectiveStartDate { get; set; }
        [Required]
        public DateTime EffectiveEndDate { get; set; }
        public string Remark { get; set; }
    }
}
