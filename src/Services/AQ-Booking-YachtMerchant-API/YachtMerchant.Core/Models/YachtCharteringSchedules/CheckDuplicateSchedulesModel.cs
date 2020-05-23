using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Models.YachtCharteringSchedules
{
    public class CheckDuplicateSchedulesModel
    {
        public long CharteringId { get; set; }
        public int YachtId { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }

    }

    public class CheckDuplicateUserSchedulesModel
    {
        public long CharteringId { get; set; }
        public int YachtId { get; set; }
        public Guid UserId { get; set; }

    }

    public class CheckDuplicateRoleSchedulesModel
    {
        public long CharteringId { get; set; }
        public int YachtId { get; set; }
        public int RoleId { get; set; }

    }
}
