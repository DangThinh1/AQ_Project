using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Admin.Core.Models.MembershipPrivileges
{
    public class MembershipPrivilegesViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
    }
}
