using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Identity.Core.Models.Authentications;
using Identity.Infrastructure.Services.Interfaces;
using Identity.Core.Conts;
using APIHelpers.Response;

namespace AQBooking.Identity.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticateService _authervice;
        public AuthController(IAuthenticateService authervice)
        {
            _authervice = authervice;
        }
 
        [HttpPost("Auth")]
        public IActionResult Auth([FromBody]AuthModel authModel)
        {
            var result = _authervice.AuthenticateByEmail(authModel.Email, authModel.Password);
            return Ok(result);
        }

        [HttpPost("FacebookAuth")]
        public IActionResult FacebookAuth([FromBody]FacebookAuthModel authModel)
        {
            authModel.UserId = authModel.UserId;
            authModel.AccessToken = authModel.AccessToken;
            var result = _authervice.AuthenticateByFaceBook(authModel.AccessToken, authModel.UserId);
            return Ok(result);
        }
        [HttpPost("GoogleAuth")]
        public IActionResult GoogleAuth([FromBody]GoogleAuthenticateModel authModel)
        {
            if (!ModelState.IsValid)
                return Ok(BaseResponse<string>.BadRequest());
            var result = _authervice.AuthenticateByGoogle(authModel);
            return Ok(result);
        }

        [HttpPost("EmailAuth")]
        public IActionResult EmailAuth([FromBody]AuthModel authModel)
        {
            var result = _authervice.AuthenticateByEmail(authModel.Email, authModel.Password);
            return Ok(result);
        }
    }
}