using AQBooking.Admin.Core.Models.Permission;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQBooking.Admin.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    public class PermissionController : ControllerBase
    {
        #region Fields

        public readonly IPermissionService _permissionService;
        //public readonly AdminApiLogger _logger;

        #endregion Fields

        #region Ctor

        public PermissionController(IPermissionService permissionService/*, AdminApiLogger logger*/)
        {
            this._permissionService = permissionService;
            //this._logger = logger;
        }

        #endregion Ctor

        #region Methods

        [HttpGet]
        [Route("Modules/ByRole")]
        public IActionResult GetModuleByRole()
        {
            try
            {
                var result = _permissionService.GetListModule().Result;
                if (result.Count == 0)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                //await _logger.InsertLog("Permission", "GetSideBarItem", APIHelpers.LogType.Error, ex.Message, ex.InnerException.Message);
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("Modules/ByControllerName/{controllerName}")]
        public IActionResult GetModuleByControllerName(string controllerName)
        {
            try
            {
                var result = _permissionService.GetModuleByControllerName(controllerName).Result;
                if (result == null)
                    return NoContent();
                return Ok(result);
            }
            catch (Exception ex)
            {
                //await _logger.InsertLog("Permission", "GetPage", APIHelpers.LogType.Error, ex.Message, ex.InnerException.Message);
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("Modules/ById/{id}")]
        public IActionResult GetModuleById(int id)
        {
            try
            {
                var result = _permissionService.GetModuleByID(id).Result;
                if (result == null)
                    return NoContent();
                return Ok(result);
            }
            catch (Exception ex)
            {
                //await _logger.InsertLog("Permission", "GetModuleById", APIHelpers.LogType.Error, ex.Message, ex.InnerException.Message);
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("Pages/ByModuleId/{moduleId}")]
        public async Task<IActionResult> GetPageByModuleID(int moduleId)
        {
            try
            {
                var result = await _permissionService.GetPageByRoleAndModuleID(moduleId);
                if (result.Count == 0)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        //Load and set permission
        [HttpGet]
        [Route("ModulePageFunctions")]
        public IActionResult GetModulePageFunction()
        {
            try
            {
                var res = _permissionService.LoadModulePageFunction().Result;
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPut]
        [Route("PageFunctions")]
        public IActionResult UpdatePageFunction([FromBody]List<UpdatePageFunctionModel> model)
        {
            try
            {
                var res = _permissionService.UpdatePageFunction(model).Result;
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("ListFunctions")]
        public IActionResult GetListFunction()
        {
            try
            {
                var res = _permissionService.GetListFunction().Result;
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("PageFunctions")]
        public IActionResult GetPageFunction()
        {
            try
            {
                var res = _permissionService.GetPageFunction().Result;
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("RolePageFunctions")]
        public IActionResult GetRolePageFunction()
        {
            try
            {
                var res = _permissionService.LoadRolePageFunction().Result;
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("Pages/CheckRolePageFunction")]
        public IActionResult CheckRolePageFunction(int functionId, string controllerName, string actionName)
        {
            try
            {
                var result = _permissionService.CheckRolePageFunction(functionId, controllerName, actionName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [Route("RoleFunctions/{roleId}")]
        public IActionResult GetRoleFunction(int roleId)
        {
            try
            {
                var res = _permissionService.GetRoleFunction(roleId).Result;
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPut]
        [Route("RoleFunctions")]
        public IActionResult UpdateRoleFunction([FromBody]List<UpdateRoleFunctionModel> model)
        {
            try
            {
                var res = _permissionService.UpdateRoleFunction(model).Result;
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        #endregion Methods
    }
}