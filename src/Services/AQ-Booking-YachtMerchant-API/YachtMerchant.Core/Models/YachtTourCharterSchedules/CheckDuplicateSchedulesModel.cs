using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtTourCharterSchedules
{
    public class CheckDuplicateSchedulesModel
    {
        public long TourCharterFid { get; set; }
        public int YachtId { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }

    }

    public class CheckDuplicateUserSchedulesModel
    {
        public long TourCharterFid { get; set; }
        public int YachtId { get; set; }
        public Guid UserId { get; set; }

    }

    public class CheckDuplicateRoleSchedulesModel
    {
        public long TourCharterFid { get; set; }
        public int YachtId { get; set; }
        public int RoleId { get; set; }

    }
}
