using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models.Permission;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class PermissionService : ServiceBase, IPermissionService
    {
        #region Fields
        #endregion Fields

        #region Ctor

        public PermissionService(AQConfigContext dbConfigContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbConfigContext, workContext, mapper)
        {
        }

        #endregion Ctor

        #region Methods

        public async Task<List<PermissionModuleModel>> GetListModule()
        {
            //RoleID will update after have function get current user
            var roleId = _workContext.UserRoleId;
            var result = new List<PermissionModuleModel>();
            if (roleId == Convert.ToInt32(UserRoleEnum.Developer))
            {
                result = await Task.Run(() => (from m in _dbConfigContext.AuthorizationModules.AsNoTracking().Where(x => x.IsShow == true)
                                               orderby m.OrderBy
                                               select new PermissionModuleModel
                                               {
                                                   ID = m.ModuleId,
                                                   DisplayName = m.DisplayName,
                                                   DisplayNameResKey = m.DisplayNameResKey,
                                                   ControllerName = m.ControllerName,
                                                   Icon = m.Icon
                                               }).ToList());
            }
            else
            {
                result = await Task.Run(() => (from m in _dbConfigContext.AuthorizationModules.AsNoTracking()
                                               join p in _dbConfigContext.AuthorizationPages.AsNoTracking() on m.ModuleId equals p.ModuleFid
                                               join r in _dbConfigContext.AuthorizationRoles.AsNoTracking() on p.PageId equals r.PageFid
                                               where r.DesignationId == roleId && m.IsShow == true
                                               orderby m.OrderBy
                                               select new PermissionModuleModel
                                               {
                                                   ID = m.ModuleId,
                                                   DisplayName = m.DisplayName,
                                                   DisplayNameResKey = m.DisplayNameResKey,
                                                   ControllerName = m.ControllerName,
                                                   Icon = m.Icon
                                               }).Distinct().ToList());
            }

            return result;
        }

        public async Task<List<PermissionPageModel>> GetPageByRoleAndModuleID(int moduleID)
        {
            //RoleID will update after have function get current user
            var roleId = _workContext.UserRoleId;
            var result = new List<PermissionPageModel>();
            if (roleId == Convert.ToInt32(UserRoleEnum.Developer))
            {
                result = await Task.Run(() => (from m in _dbConfigContext.AuthorizationPages.AsNoTracking()
                                               where m.Active == true && m.ModuleFid == moduleID
                                               select new PermissionPageModel
                                               {
                                                   Id = m.PageId,
                                                   PageName = m.PageName,
                                                   PageNameResKey = m.PageNameResKey,
                                                   Controller = m.Controller,
                                                   Icon = m.Icon,
                                                   Action = m.Action,
                                                   ModuleId = moduleID,
                                                   ParentId = m.ParentFid
                                               }).ToList());
            }
            else
            {
                result = await Task.Run(() => (from m in _dbConfigContext.AuthorizationPages.AsNoTracking()
                                               join r in _dbConfigContext.AuthorizationRoles.AsNoTracking() on m.PageId equals r.PageFid
                                               where r.DesignationId == roleId && m.ModuleFid == moduleID && m.Active == true
                                               select new PermissionPageModel
                                               {
                                                   Id = m.PageId,
                                                   PageName = m.PageName,
                                                   PageNameResKey = m.PageNameResKey,
                                                   Controller = m.Controller,
                                                   Icon = m.Icon,
                                                   Action = m.Action,
                                                   ModuleId = moduleID,
                                                   ParentId = m.ParentFid
                                               }).Distinct().ToList());
            }
            return result;
        }

        public bool CheckRolePageFunction(int functionId, string controllerName, string actionName)
        {
            try
            {
                var roleId = _workContext.UserRoleId;
                var pageId = (from p in _dbConfigContext.AuthorizationPages.AsNoTracking()
                              join r in _dbConfigContext.AuthorizationRoles.AsNoTracking() on p.PageId equals r.PageFid
                              where r.DesignationId == roleId
                              && p.Controller == controllerName.Trim()
                              && (string.IsNullOrEmpty(actionName) || p.Action == actionName.Trim())
                              && p.Active == true
                              select p.PageId).FirstOrDefault();

                var functionRoleItem = (from r in _dbConfigContext.AuthorizationRoles.AsNoTracking()
                                        where r.DesignationId == roleId && r.PageFid == pageId && r.FunctionFid == functionId
                                        select r).SingleOrDefault();
                if (functionRoleItem != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<AuthorizationModules> GetModuleByControllerName(string controllerName)
        {
            return await Task.Run(() => _dbConfigContext.AuthorizationModules.AsNoTracking().Where(x => x.ControllerName == controllerName && x.IsShow == true && x.Active == true).SingleOrDefault());
        }

        public async Task<AuthorizationModules> GetModuleByID(int id)
        {
            return await Task.Run(() => _dbConfigContext.AuthorizationModules.AsNoTracking().FirstOrDefault(x => x.ModuleId == id));
        }

        public async Task<List<AuthorizationModules>> GetAllModule()
        {
            return await Task.Run(() => _dbConfigContext.AuthorizationModules.AsNoTracking().ToList());
        }

        public async Task<List<AuthorizationPages>> GetAllPage()
        {
            return await Task.Run(() => _dbConfigContext.AuthorizationPages.AsNoTracking().ToList());
        }

        public async Task<List<AuthorizationFunctions>> GetAllFunction()
        {
            return await Task.Run(() => _dbConfigContext.AuthorizationFunctions.AsNoTracking().ToList());
        }

        public async Task<List<AuthorizationPageFunctions>> GetListFunctionOfPage(int pageID)
        {
            return await Task.Run(() => _dbConfigContext.AuthorizationPageFunctions.AsNoTracking().Where(x => x.PageId == pageID).ToList());
        }

        //Load structure of page function
        //Page function include module, page per mudule, function per page
        public async Task<List<PermissionPageFunctionModel>> LoadModulePageFunction()
        {
            var res = await Task.Run(() => (from m in _dbConfigContext.AuthorizationModules.AsNoTracking()
                                            where m.IsShow == true && m.Active == true
                                            select new PermissionPageFunctionModel
                                            {
                                                ModuleId = m.ModuleId,
                                                ModuleName = m.DisplayName,
                                                ModuleNameReskey = m.DisplayNameResKey,
                                                Icon = m.Icon,
                                                ListPage = (from p in _dbConfigContext.AuthorizationPages.AsNoTracking()
                                                            where p.ModuleFid == m.ModuleId
                                                            select new PageModel
                                                            {
                                                                PageId = p.PageId,
                                                                PageName = p.PageName,
                                                                PageNameResKey = p.PageNameResKey,
                                                                Icon = p.Icon,
                                                                ListFunction = (from f in _dbConfigContext.AuthorizationFunctions.AsNoTracking()
                                                                                where f.Active == true
                                                                                select new FunctionModel
                                                                                {
                                                                                    FunctionId = f.FuntionId,
                                                                                    FunctionName = f.FunctionName,
                                                                                    FunctionNameReskey = f.FunctionNameResKey
                                                                                }).ToList()
                                                            }).ToList()
                                            }).ToList());
            return res;
        }

        //Update new page function
        public async Task<bool> UpdatePageFunction(List<UpdatePageFunctionModel> model)
        {
            using (var trans = _dbConfigContext.Database.BeginTransaction())
            {
                try
                {
                    var result = false;
                    var delAll = await DeleteAllPageFunction();
                    if (delAll)
                    {
                        foreach (var item in model)
                        {
                            foreach (var functionId in item.ListFunctionId)
                            {
                                var pageFunctionItem = new AuthorizationPageFunctions();
                                pageFunctionItem.PageId = item.PageId;
                                pageFunctionItem.FunctionId = functionId;
                                await _dbConfigContext.AuthorizationPageFunctions.AddAsync(pageFunctionItem);
                                await _dbConfigContext.SaveChangesAsync();
                            }
                        }
                        
                        trans.Commit();
                        result = true;
                    }

                    trans.Dispose();
                    return result;
                }
                catch
                {
                    trans.Rollback();
                    trans.Dispose();
                    throw;
                }
            }    
        }

        //Get list function
        public async Task<List<AuthorizationFunctions>> GetListFunction()
        {
            var lstFunction = await Task.Run(() => _dbConfigContext.AuthorizationFunctions.AsNoTracking().Where(x => x.Active == true).ToList());
            return lstFunction;
        }

        //Get page function
        public async Task<List<AuthorizationPageFunctions>> GetPageFunction()
        {
            var lstPageFunction = await Task.Run(() => _dbConfigContext.AuthorizationPageFunctions.AsNoTracking().ToList());
            return lstPageFunction;
        }

        //Delete all page function
        public async Task<bool> DeleteAllPageFunction()
        {
            try
            {
                var lstPageFunction = _dbConfigContext.AuthorizationPageFunctions.AsNoTracking().ToList();
                if (lstPageFunction.Count == 0)
                    return true;
                _dbConfigContext.AuthorizationPageFunctions.RemoveRange(lstPageFunction);
                var flag = await _dbConfigContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Load role to config
        public async Task<PermissionRoleModel> LoadRolePageFunction()
        {
            var result = new PermissionRoleModel();
            var lstModule = await Task.Run(() => (from m in _dbConfigContext.AuthorizationModules.AsNoTracking()
                                                  where m.Active == true && m.IsShow == true
                                                  select new PermissionPageFunctionModel
                                                  {
                                                      ModuleId = m.ModuleId,
                                                      ModuleName = m.DisplayName,
                                                      ModuleNameReskey = m.DisplayNameResKey,
                                                      Icon = m.Icon,
                                                      ListPage = (from p in _dbConfigContext.AuthorizationPages.AsNoTracking()
                                                                  where p.Active == true && p.ModuleFid == m.ModuleId
                                                                  select new PageModel
                                                                  {
                                                                      PageId = p.PageId,
                                                                      PageName = p.PageName,
                                                                      PageNameResKey = p.PageNameResKey,
                                                                      Icon = p.Icon,
                                                                      ListFunction = (from pf in _dbConfigContext.AuthorizationPageFunctions.AsNoTracking()
                                                                                      join f in _dbConfigContext.AuthorizationFunctions.AsNoTracking() on pf.FunctionId equals f.FuntionId
                                                                                      where pf.PageId == p.PageId
                                                                                      select new FunctionModel
                                                                                      {
                                                                                          FunctionId = f.FuntionId,
                                                                                          FunctionName = f.FunctionName,
                                                                                          FunctionNameReskey = f.FunctionNameResKey
                                                                                      }).ToList()
                                                                  }).ToList()
                                                  }).ToList());
            result.LstModule = lstModule;
            return result;
        }

        public async Task<List<AuthorizationRoles>> GetRoleFunction(int roleId)
        {
            var result = await Task.Run(() => (from i in _dbConfigContext.AuthorizationRoles.AsNoTracking() where i.DesignationId == roleId select i).ToList());
            return result;
        }

        //Update role function
        public async Task<bool> UpdateRoleFunction(List<UpdateRoleFunctionModel> model)
        {
            using (var trans = _dbConfigContext.Database.BeginTransaction())
            {
                try
                {
                    var result = false;
                    var delAll = await DeleteRolePageFunctionById(model[0].RoleId);
                    if (delAll)
                    {
                        foreach (var item in model)
                        {
                            await _dbConfigContext.AuthorizationRoles.AddAsync(new AuthorizationRoles { DesignationId = item.RoleId, PageFid = item.PageId, FunctionFid = item.FunctionId });
                            await _dbConfigContext.SaveChangesAsync();
                        }

                        result = true;
                        trans.Commit();
                    }

                    trans.Dispose();
                    return result;
                }
                catch
                {
                    trans.Rollback();
                    trans.Dispose();
                    throw;
                }
            } 
        }

        //Delete role page function by roleid
        public async Task<bool> DeleteRolePageFunctionById(int id)
        {
            try
            {
                var lstRolePageFunction = _dbConfigContext.AuthorizationRoles.AsNoTracking().Where(x => x.DesignationId == id).ToList();
                if (lstRolePageFunction.Count == 0)
                    return true;
                _dbConfigContext.AuthorizationRoles.RemoveRange(lstRolePageFunction);
                var flag = await _dbConfigContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        #endregion Methods
    }
}