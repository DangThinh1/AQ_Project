using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Core.Models.MembershipPrivileges;
using AQBooking.Admin.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IMembershipPrivilegeService
    {
        IPagedList<MembershipPrivilegesViewModel> SearchMembershipPrivileges(MembershipPrivilegesSearchModel searchModel);
        MembershipPrivilegesCreateModel GetMembershipPrivilegesById(int id);
        Task<BasicResponse> CreateOrUpdateMembershipPrivileges(MembershipPrivilegesCreateModel model);
        Task<BasicResponse> DeleteMembershipPrivileges(int id);
    }
}
