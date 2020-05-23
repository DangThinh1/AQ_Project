using System;
using APIHelpers.Response;
using Identity.Core.Models.Roles;
using System.Collections.Generic;
using Identity.Core.Services.Interfaces;

namespace Identity.Core.Services.Implements
{
    public class RolesRequestServices : RequestServiceBase, IRolesRequestServices
    {
        public RolesRequestServices(IUsersContext usersContext) : base(usersContext)
        {
        }

        public BaseResponse<List<RoleViewModel>> GetRolesByDomain(string domainId, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Roles/Domain/{domainId}" : api,
                };
                var response = Get<List<RoleViewModel>>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RoleViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<RoleViewModel>> GetSubordinateRole(int id, string host = "", string api = "")
        {
            try
            {
                var requestModel = new ApiRequestModel()
                {
                    Host = host,
                    Api = string.IsNullOrEmpty(api) ? $"api/Roles/Superior/{id}" : api,
                };
                var response = Get<List<RoleViewModel>>(requestModel);
                return response;
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RoleViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

    }
}
