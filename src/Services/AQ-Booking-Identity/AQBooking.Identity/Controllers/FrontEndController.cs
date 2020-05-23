using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Identity.Core.Models.Users;
using Identity.Core.Services.Interfaces;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrontEndController : ControllerBase
    {
        private readonly ISignInRequestService _signInRequestService;
        private readonly IAccountsRequestService _accountsRequestService;
        private readonly IAuthenticateRequestService _authenticateRequestService;
        
        public FrontEndController(IAuthenticateRequestService authenticateRequestService, 
            IAccountsRequestService accountsRequestService,
            ISignInRequestService signInRequestService)
        {
            _signInRequestService = signInRequestService;
            _signInRequestService.BaseUrl = "https://localhost:44393/";

            _accountsRequestService = accountsRequestService;
            _accountsRequestService.BaseUrl = "https://localhost:44393/";

            _authenticateRequestService = authenticateRequestService;
            _authenticateRequestService.BaseUrl = "https://localhost:44393/";
        }

        [HttpGet("checkToken")]
        public IActionResult Auth(string token)
        {
            var result = _signInRequestService.IsAllowedToken(token);
            return Ok(result);
        }

        [HttpGet("SignOutAllDevices/{key}")]
        public IActionResult SignOutAllDevices(string key)
        {
            var result = _signInRequestService.SignOutAllDevicesRequest(key);
            return Ok(result);
        }

        [HttpGet("Auth")]
        public IActionResult Auth(string email, string password)
        {
            var result = _authenticateRequestService.Authenticate(email, password);
            return Ok(result);
        }

        [HttpGet("Accounts/MyProfile")]
        public IActionResult MyProfile()
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.MyProfile();
            return Ok(result);
        }

        [HttpGet("Accounts/UserProfile")]
        public IActionResult UserProfile()
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.UserProfile("onedam@gmail.com");
            return Ok(result);
        }

        [HttpGet("Accounts/AllProfile")]
        public IActionResult AllProfile()
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.AllProfiles();
            return Ok(result);
        }

        [HttpPost("Accounts/SearchProfile")]
        public IActionResult SearchProfile([FromBody]UserSearchModel model)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.SearchProfiles(model);
            return Ok(result);
        }

        [HttpPost("Accounts/Create")]
        public IActionResult Create([FromBody]RegisterModel model)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.Create(model);
            return Ok(result);
        }

        [HttpPost("Accounts/Register")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.Register(model);
            return Ok(result);
        }

        [HttpPut("Accounts/Lang")]
        public IActionResult Lang(string key, int lang)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.ChangeLanguage(key, lang);
            return Ok(result);
        }

        [HttpPut("Accounts/Avatar")]
        public IActionResult Avatar(string key, int image)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.UpdateAvatar(key, image);
            return Ok(result);
        }

        [HttpPut("Accounts/Status")]
        public IActionResult Status(string key, bool image)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.ChangeStatus(key, image);
            return Ok(result);
        }

        [HttpPut("Accounts/Password")]
        public IActionResult Status(string key, string image)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.ChangePassword(key, image);
            return Ok(result);
        }

        [HttpPut("Accounts/Delete")]
        public IActionResult Delete(string key)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.Delete(key);
            return Ok(result);
        }

        [HttpPut("Accounts/Pro/{key}")]
        public IActionResult Pro(string key, [FromBody] UserProfileUpdateModel model)
        {
            MockToken();
            var user = this.User;
            var result = _accountsRequestService.UpdateProfile(key, model);
            return Ok(result);
        }



        private void MockToken()
        {
            var hardCodeToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiVW5pcXVlSWQiOiJPTTE4NTVIM0s2REkiLCJlbWFpbCI6Im9uZWRhbUBnbWFpbC5jb20iLCJ1bmlxdWVfbmFtZSI6InN0cmluZyBzdHJpbmciLCJyb2xlIjoiIiwiUm9sZUlkIjoiMCIsIlJlZnJlc2hUb2tlbiI6IlJSM0c3VlI3MzhQUyIsIk1lcmNoYW50RmlkIjoiMCIsIm5iZiI6MTU2Mzc2ODYxOSwiZXhwIjoxOTIzNzY4NjE5LCJpYXQiOjE1NjM3Njg2MTksImlzcyI6IkFRSWRlbnRpdHlTZXJ2ZXIiLCJhdWQiOiJBUUlkZW50aXR5U2VydmVyIn0.9apuoope6vLas32cwikZG9O2BA2jWG2pO0Rx3c-htWQ";
            User.Identities.FirstOrDefault().AddClaim(new Claim(ClaimTypes.Rsa, hardCodeToken));
        }
    }
}