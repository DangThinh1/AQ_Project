using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Admin.Core.Models.MembershipPrivileges
{
    public class MembershipPrivilegesCreateModel
    {
      
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int RoleFID { get; set; }
        public string RoleName { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public List<MembershipPrivilegesDetailCreateModel> DataDetail { get; set; }
        public MembershipPrivilegesCreateModel()
        {
            DataDetail = new List<MembershipPrivilegesDetailCreateModel>();
        }
    }
}
