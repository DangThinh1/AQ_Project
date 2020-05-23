using System.Linq;
using Identity.Core.Conts;
using APIHelpers.Response;
using Identity.Core.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Models.Users;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AQBooking.Identity.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region HttpGet Api

        [AllowAnonymous]
        [HttpGet("Accounts/VerifyEmailForCreate/{email}")]
        public IActionResult VerifyEmailForCreate(string email)
        {
            var result = _accountService.VerifyEmailForCreate(email);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("Accounts/VerifyEmailForSignIn/{email}")]
        public IActionResult VerifyEmailForSignIn(string email)
        {
            var result = _accountService.VerifyEmailForSignIn(email);
            return Ok(result);
        }

        [HttpGet("Accounts/MyProfile")]
        public IActionResult GetMyProfile()
        {
            var userPrincipal = UserContextHelper.UserPrincipal;
            var userProfiles = UserContextHelper.UserProfiles;
            var uid = GetUniqueId();
            var myProfile = _accountService.GetUserProfile(uid);
            return Ok(myProfile);
        }

        [HttpGet("Accounts/{key}")]
        public IActionResult GetProfile(string key)
        {
            var myProfile = _accountService.GetUserProfile(key);
            return Ok(myProfile);
        }

        [HttpGet("Accounts")]
        public IActionResult AllProfiles()
        {
            var model = new UserSearchModel() { //=> Set filter value to get all record
                PageIndex = 1,
                PageSize = -1,
                RoleIds = null
            };
            var profiles = _accountService.Search(model);
            return Ok(profiles);
        }

        #endregion HttpGet Api

        #region HttpPut Api
        [HttpPut("Accounts/{key}")]
        public IActionResult UpdateProfile(string key, [FromBody] UserProfileUpdateModel model)
        {
            var result = _accountService.UpdateProfile(key, model);
            return Ok(result);
        }

        [HttpPut("Accounts/{key}/Language/{lang}")]
        public IActionResult ChangeLanguage(string key, int lang)
        {
            if (lang < 1)
                return Ok(BaseResponse<bool>.BadRequest());
            var result = _accountService.UpdateProperty(key, "LangId", lang);
            return Ok(result);
        }

        [HttpPut("Accounts/{key}/Avatar/{imageId}")]
        public IActionResult ChangeAvatar(string key, int imageId)
        {
            var result = _accountService.UpdateProperty(key, "ImageId", imageId);
            return Ok(result);
        }

        [HttpPut("Accounts/{key}/Status/{status}")]
        public IActionResult ChangeStatus(string key, bool status)
        {
            var result = _accountService.UpdateProperty(key, "IsActivated", status);
            return Ok(result);
        }

        [HttpPut("Accounts/Password/{password}")]
        public IActionResult ChangePassword(string password)
        {
            var uid = GetUniqueId();
            var result = _accountService.ChangePassword(uid, password);
            return Ok(result);
        }

        #endregion HttpPut Api

        #region HttpPost Api
        [HttpPost("Accounts/Searching")]
        public IActionResult SearchingProfiles([FromBody]UserSearchModel model)
        {
            var profiles = _accountService.Search(model);
            return Ok(profiles);
        }

        [HttpPost("Accounts/Searching/RoleIds")]
        public IActionResult SearchingProfilesByRoleIds([FromBody]List<int> roleIds)
        {
            var model = new UserSearchModel() {
                RoleIds = roleIds,
                PageIndex = 1,
                PageSize = -1
            };
            var profiles = _accountService.Search(model);
            return Ok(profiles);
        }

        [AllowAnonymous]
        [HttpPost("Accounts/Register")]
        public IActionResult Register([FromBody]UserCreateModel register)
        {
            var result = _accountService.Resigster(register);
            return Ok(result);
        }

        [HttpPost("Accounts")]
        public IActionResult Create([FromBody]UserCreateModel createModel)
        {
            var userId = JwtTokenHelper.GetSignInProfile(User).UserId;
            var result = _accountService.Resigster(createModel, userId);
            return Ok(result);
        }
        #endregion HttpPost Api

        #region HttpDelete Api
        [HttpDelete("Accounts/{key}")]
        public IActionResult Delete(string key)
        {
            var result = _accountService.UpdateProperty(key, "Deleted", true);
            return Ok(result);
        }
        #endregion HttpDelete Api

        #region Private Method

        private string GetUniqueId() => GetClaimValue(User, "UniqueId");

        private string GetClaimValue(ClaimsPrincipal principal, string claimType)
        {
            try
            {
                return principal.Identities.FirstOrDefault().FindFirst(claimType).Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}