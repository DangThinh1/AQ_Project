using Identity.Core.Common;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AQEncrypts;
using Identity.Core.Conts;
using Identity.Core.Models.Usertokens;
using APIHelpers.Response;
using System.Collections.Generic;
using Identity.Core.Portal.Models.SigninControls;

namespace Identity.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SignInController : ControllerBase
    {
        private readonly ISignInService _signInService;
        private readonly IUserTokenService _userTokenService;
        private readonly ISinginControlService _singinControlService;

        public SignInController(ISignInService signInService, 
                                IUserTokenService userTokenService, 
                                ISinginControlService singinControlService)
        {
            _signInService = signInService;
            _userTokenService = userTokenService;
            _singinControlService = singinControlService;
        }

        [AllowAnonymous]
        [HttpGet("SignIn/Controls/{domainUid}")]
        public IActionResult FindByDomainUid(string domainUid)
        {
            var result = _singinControlService.FindByDomainUid(domainUid);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpDelete("SignIn/UserToken/{uid}")]
        public IActionResult DeleteUserTokenByUserId(string uid)
        {
            var result = _userTokenService.DeleteUserTokenByUserId(uid);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("SignIn/UserToken")]
        public IActionResult CreateUserToken([FromBody]UserTokenCreateModel model)
        {
            if(model == null)
            {
                return Ok(BaseResponse<string>.BadRequest());
            }
            var result = _userTokenService.Create(model.AccessToken, model.ReturnUrl);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("SignIn/UserToken/{id}")]
        public IActionResult FindUserToken(string id)
        {
            var result = _userTokenService.FindUserToken(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("SignIn/TokenVerification")]
        public IActionResult VerifyToken([FromBody]string token)
        {
            var tokenDecrypted = token.Decrypt();
            if (string.IsNullOrEmpty(tokenDecrypted) || tokenDecrypted == "0")
                return BadRequest("Token is invalid");
            var result = _signInService.IsAllowedToken(tokenDecrypted);
            return Ok(result);
        }

        [HttpGet("SignIn/SignOutAllDevices")]
        public IActionResult SignOutAllDevices()
        {
            var key = JwtTokenHelper.GetClaimValue(User, ClaimConstant.Email) ?? string.Empty;
            var result = _signInService.SignOutAllDevice(key);
            return Ok(result);
        }
    }
}