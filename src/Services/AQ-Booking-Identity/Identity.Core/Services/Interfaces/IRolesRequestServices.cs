using APIHelpers.Response;
using Identity.Core.Models.Roles;
using System.Collections.Generic;

namespace Identity.Core.Services.Interfaces
{
    public interface IRolesRequestServices : IRequestServiceBase
    {
        BaseResponse<List<RoleViewModel>> GetRolesByDomain(string domainId, string host = "", string api = "");
        BaseResponse<List<RoleViewModel>> GetSubordinateRole(int id, string host = "", string api = "");
    }
}
