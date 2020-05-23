using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Admin.Core.Models.MembershipPrivileges
{
    public class MembershipPrivilegesSearchModel : PagableModel
    {
        public string Name { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? Role { get; set; }
    }
}
