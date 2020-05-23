using Identity.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Identity.Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;

namespace Identity.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [RequiredAccountType]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdminsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AdminsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("Accounts/{key}")]
        public IActionResult GetProfile(string key)
        {
            var myProfile = _accountService.GetUserProfile(key);
            return Ok(myProfile);
        }

        [HttpGet("Accounts")]
        public IActionResult AllUsers()
        {
            var model = new UserSearchModel()
            { //=> Set filter value to get all record
                PageIndex = 1,
                PageSize = -1,
                RoleIds = null
            };
            var profiles = _accountService.Search(model);
            return Ok(profiles);
        }

        [HttpPost("Accounts/Searching")]
        public IActionResult SearchingProfiles([FromBody]UserSearchModel model)
        {
            var profiles = _accountService.Search(model);
            return Ok(profiles);
        }

        [HttpPost("Accounts/Create")]
        public IActionResult Register([FromBody]UserCreateModel register)
        {
            register.RoleId = "1";
            var result = _accountService.Resigster(register);
            return Ok(result);
        }

        [HttpPost("Accounts/Searching/RoleIds")]
        public IActionResult SearchingProfilesByRoleIds([FromBody]List<int> roleIds)
        {
            var model = new UserSearchModel()
            {
                RoleIds = roleIds,
                PageIndex = 1,
                PageSize = -1
            };
            var profiles = _accountService.Search(model);
            return Ok(profiles);
        }

        [HttpDelete("Accounts/{key}")]
        public IActionResult Delete(string key)
        {
            var result = _accountService.UpdateProperty(key, "Deleted", true);
            return Ok(result);
        }
    }
}
