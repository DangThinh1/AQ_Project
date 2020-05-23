using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharteringSchedules
{
    public class YachtCharteringSchedulesCreateModel
    {
        [Required]
        public long CharteringFid { get; set; }
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
