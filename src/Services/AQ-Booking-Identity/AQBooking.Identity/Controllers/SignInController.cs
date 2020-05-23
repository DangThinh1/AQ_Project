//using AQEncrypts;
using Identity.Core.Common;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Identity.Core.Conts;

namespace Identity.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SignInController : ControllerBase
    {
        private readonly ISignInService _signInService;
        public SignInController(ISignInService signInService)
        {
            _signInService = signInService;
        }

        [AllowAnonymous]
        [HttpPost("SignIn/TokenVerification")]
        public IActionResult VerifyToken([FromBody]string token)
        {
            var tokenDecrypted = token;
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