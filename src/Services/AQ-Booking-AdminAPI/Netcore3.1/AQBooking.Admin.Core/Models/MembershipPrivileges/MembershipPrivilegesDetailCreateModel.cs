using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Admin.Core.Models.MembershipPrivileges
{
    public class MembershipPrivilegesDetailCreateModel
    {
        public long? ID { get; set; }
        public int? PrivilegeFID { get; set; }
        public string BookingDomainUniqueID { get; set; }
        public int BookingDomainFID { get; set; }
        public string BookingDomainName { get; set; }
        public double? DiscountPercent { get; set; }
        public string Remark { get; set; }
    }
}
