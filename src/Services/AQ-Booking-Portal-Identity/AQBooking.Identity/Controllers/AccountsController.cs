using System.Linq;
using APIHelpers.Response;
using Identity.Core.Conts;
using Identity.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AQBooking.Identity.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private string Email
        {
            get
            {
                try
                {
                    return User.Identities.FirstOrDefault().FindFirst(ClaimConstant.Email).Value;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Accounts/VerifyEmailForCreate/{email}")]
        public IActionResult VerifyEmailForCreate(string email)
        {
            var result = _accountService.VerifyEmailForCreate(email);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Accounts/VerifyEmailForSignIn/{email}")]
        public IActionResult VerifyEmailForSignIn(string email)
        {
            var result = _accountService.VerifyEmailForSignIn(email);
            return Ok(result);
        }

        [HttpGet]
        [Route("Accounts/MyProfile")]
        public IActionResult GetMyProfile()
        {
            var myProfile = _accountService.GetUserProfile(Email);
            return Ok(myProfile);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Accounts/Register")]
        public IActionResult Register([FromBody]UserCreateModel register)
        {
            register.RoleId = "1";
            var result = _accountService.Resigster(register);
            return Ok(result);
        }

        [HttpPut]
        [Route("Accounts")]
        public IActionResult UpdateProfile(string key,[FromBody] UserProfileUpdateModel model)
        {
            var result = _accountService.UpdateProfile(Email, model);
            return Ok(result);
        }

        [HttpPut]
        [Route("Accounts/{key}/Language/{lang}")]
        public IActionResult ChangeLanguage(string key, int lang)
        {
            if (lang < 1)
                return Ok(BaseResponse<bool>.BadRequest());
            var result = _accountService.UpdateProperty(Email,"LangId", lang);
            return Ok(result);
        }

        [HttpPut]
        [Route("Accounts/{key}/Avatar/{imageId}")]
        public IActionResult ChangeAvatar(string key, int imageId)
        {
            var result = _accountService.UpdateProperty(Email, "ImageId", imageId);
            return Ok(result);
        }

        [HttpPut]
        [Route("Accounts/{key}/Status/{status}")]
        public IActionResult ChangeStatus(string key, bool status)
        {
            var result = _accountService.UpdateProperty(Email, "IsActivated", status);
            return Ok(result);
        }

        [HttpPut]
        [Route("Accounts/Password/{password}")]
        public IActionResult ChangePassword(string password)
        {
            var result = _accountService.ChangePassword(Email, password);
            return Ok(result);
        }
    }
}