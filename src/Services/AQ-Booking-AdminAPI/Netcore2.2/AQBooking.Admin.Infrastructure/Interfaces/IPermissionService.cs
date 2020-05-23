using AQBooking.Admin.Core.Models.Permission;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IPermissionService
    {
        //Get permission
        Task<List<PermissionModuleModel>> GetListModule();
        Task<List<PermissionPageModel>> GetPageByRoleAndModuleID(int moduleID);
        Task<AuthorizationModules> GetModuleByControllerName(string controllerName);
        Task<AuthorizationModules> GetModuleByID(int id);
        Task<List<AuthorizationModules>> GetAllModule();
        Task<List<AuthorizationPages>> GetAllPage();
        Task<List<AuthorizationFunctions>> GetAllFunction();
        Task<List<AuthorizationPageFunctions>> GetListFunctionOfPage(int pageID);
        bool CheckRolePageFunction(int functionId, string controllerName, string actionName);

        //Load and set permission
        Task<List<PermissionPageFunctionModel>> LoadModulePageFunction();
        Task<bool> UpdatePageFunction(List<UpdatePageFunctionModel> model);
        Task<List<AuthorizationFunctions>> GetListFunction();
        Task<List<AuthorizationPageFunctions>> GetPageFunction();
        Task<bool> DeleteAllPageFunction();
        Task<PermissionRoleModel> LoadRolePageFunction();
        Task<List<AuthorizationRoles>> GetRoleFunction(int roleId);
        Task<bool> UpdateRoleFunction(List<UpdateRoleFunctionModel> model);
        Task<bool> DeleteRolePageFunctionById(int id);
    }
}

