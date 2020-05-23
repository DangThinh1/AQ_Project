using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Conts;

namespace Identity.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("Roles/Domain/{domainId}")]
        public IActionResult GetRolesByDomain(string domainId)
        {
            var result = _roleService.GetRolesByDomain(domainId);
            return Ok(result);
        }

        [HttpGet]
        [Route("Roles")]
        public IActionResult GetAllRoles()
        {
            var result = _roleService.GetAllRoles();
            return Ok(result);
        }

        [HttpGet]
        [Route("Roles/Superior/{id}")]
        public IActionResult GetSubordinateRole(int id)
        {
            var result = _roleService.GetSubordinateRole(id);
            return Ok(result);
        }
    }
}