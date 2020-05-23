using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.MembershipPrivileges;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    public class MembershipPrivilegesController : ControllerBase
    {
        #region Fields

        private readonly IMembershipPrivilegeService _membershipPrivilegeService;

        #endregion Fields

        #region Ctor
        public MembershipPrivilegesController(IMembershipPrivilegeService membershipPrivilegeService)
        {
            _membershipPrivilegeService = membershipPrivilegeService;
        }
        #endregion Ctor

        [HttpGet]
        [Route("MembershipPrivileges")]
        public IActionResult SearchMembershipPrivileges([FromQuery]MembershipPrivilegesSearchModel searchModel)
        {
            try
            {
                var result = _membershipPrivilegeService.SearchMembershipPrivileges(searchModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }
        [HttpGet]
        [Route("MembershipPrivileges/{id}")]
        public IActionResult FindMembershipPrivilege(int id)
        {
            try
            {
                var result = _membershipPrivilegeService.GetMembershipPrivilegesById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpDelete]
        [Route("MembershipPrivileges/{id}")]
        public IActionResult DeleteMembershipPrivilege(int id)
        {
            try
            {
                var result = _membershipPrivilegeService.DeleteMembershipPrivileges(id).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

        [HttpPost]
        [Route("InsertOrUpdateMembershipPrivilege")]
        public IActionResult InsertOrUpdateMembershipPrivilege([FromBody]MembershipPrivilegesCreateModel model )
        {
            try
            {
                var result = _membershipPrivilegeService.CreateOrUpdateMembershipPrivileges(model).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.StackTrace.ToString());
            }
        }

    }
}