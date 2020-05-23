using APIHelpers.Response;
using Identity.Core.Models.Roles;
using System.Collections.Generic;
namespace Identity.Infrastructure.Services.Interfaces
{
    public interface IRoleService
    {
        BaseResponse<List<RoleViewModel>> GetSubordinateRole(int id);
        BaseResponse<List<RoleViewModel>> GetRolesByDomain(string domainFid);
        BaseResponse<List<RoleViewModel>> GetAllRoles();
    }
}
