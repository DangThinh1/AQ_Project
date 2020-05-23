using APIHelpers.Response;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Models.User;
using AQS.BookingMVC.Services.Interfaces;
using Identity.Core.Common;
using Identity.Core.Helpers;
using Identity.Core.Models.Authentications;
using Identity.Core.Models.Users;
using Identity.Core.Portal.Models.SSOAuthentication;
using Identity.Core.Portal.Services.Interfaces;
using Identity.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Controllers
{
    public class UserController : BaseController
    {
        #region Fields


        private string SSO_DOMAIN_LOGOUT_REDIRECT_URL => $"{ApiUrlHelper.SSOPortal}/User/Logout";
        private const string PORTAL_DOMAIN_ID = "DiningPortal";
        private readonly ISSOAuthenticationRequestService _sSOAuthenticationRequestService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthenticateRequestService _authenticateRequestService;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public UserController(ISSOAuthenticationRequestService sSOAuthenticationRequestService,
             IWebHostEnvironment webHostEnvironment,
             IAuthenticateRequestService authenticateRequestService,
             IUserService userService)
        {
            _sSOAuthenticationRequestService = sSOAuthenticationRequestService;
            _webHostEnvironment = webHostEnvironment;
            _authenticateRequestService = authenticateRequestService;
            _userService = userService;
        }
        #endregion

        #region Methods

        #region Login
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult Login()
        //{
        //    var url = $"{SSO_DOMAIN_LOGIN_REDIRECT_URL}?d={PORTAL_DOMAIN_ID}";
        //    return Redirect(url);
        //}
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<JsonResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(BaseResponse<bool>.BadRequest());
            }

            if (!User.Identity.IsAuthenticated)
            {
                var authResult = _authenticateRequestService.Authenticate(model.Email, model.Password);
                if (authResult.IsSuccessStatusCode)
                {
                    await SignInAsync(authResult.ResponseData);
                    return Json(BaseResponse<bool>.Success(true));
                }

                return Json(BaseResponse<bool>.NotFound(message: authResult.Message));
            }

            return Json(BaseResponse<bool>.NotFound());
        }



        #region Facebook / Google login
        [HttpPost]
        public async Task<IActionResult> FacebookLogin(FacebookAuthModel model)
        {
            if (!string.IsNullOrEmpty(model.UserId)
                && !string.IsNullOrEmpty(model.AccessToken))
            {
                var result = _authenticateRequestService.FacebookAuthenticate(model.UserId, model.AccessToken);
                if (result.IsSuccessStatusCode)
                {
                    await SignInAsync(result.ResponseData);
                    return Ok(true);
                }
                return Ok(BaseResponse<string>.BadRequest(result.Message));
            }
            return Ok(BaseResponse<string>.BadRequest());

        }
        [HttpPost]
        public async Task<IActionResult> GoogleLogin(GoogleAuthenticateModel model)
        {
            if (ModelState.IsValid)
            {

                var result = _authenticateRequestService.GoogleAuthenticate(model);
                if (result.IsSuccessStatusCode)
                {
                    await SignInAsync(result.ResponseData);
                    return Ok(true);
                }
                return Ok(BaseResponse<string>.BadRequest(result.Message));
            }
            return Ok(BaseResponse<string>.BadRequest());

        }
        #endregion

        #endregion Login

        #region Logout

        public IActionResult Logout()
        {
            var url = $"{SSO_DOMAIN_LOGOUT_REDIRECT_URL}?d={PORTAL_DOMAIN_ID}";
            return Redirect(url);
        }

        public async Task<IActionResult> LogoutNext(string i)
        {
            var findSSOAuthInfo = _sSOAuthenticationRequestService.FindByDomainId(i, PORTAL_DOMAIN_ID);
            if (findSSOAuthInfo.IsSuccessStatusCode)
            {
                await LogoutAsync();
                return LogoutNextDomain(findSSOAuthInfo.ResponseData);
            }
            return UnauthorizeError("LogoutNext");
        }

        public IActionResult LogoutEnd()
        {
            return RedirectToAction("Index", "Home");
        }

        private IActionResult LogoutNextDomain(SSOAuthenticationViewModel ssoAuthModel)
        {
            if (ssoAuthModel != null)
            {
                var redirectUrl = $"{ssoAuthModel.RedirectUrl}?i={ssoAuthModel.AuthId}";
                return Redirect(redirectUrl);
            }
            return UnauthorizeError("LogoutNextDomain");
        }

        private async Task LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        #endregion Logout

        #region New Login Popup

        [HttpPost]
        public async Task<IActionResult> LoginTemp()
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                var email = "oneyam@gmail.com";
                var password = "12341234";
                if (!User.Identity.IsAuthenticated)
                {
                    var authResult = _authenticateRequestService.Authenticate(email, password);
                    if (authResult.IsSuccessStatusCode)
                    {
                        await SignInAsync(authResult.ResponseData);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogoutTemp()
        {
            if (_webHostEnvironment.IsDevelopment())
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        #endregion Temp

        #region Register
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]UserCreateModel model)
        {
            bool res = _userService.Resigster(model);
            LoginModel login = new LoginModel
            {
                Email = model.Email,
                Password = model.Password,
            };
            if (res)
                return await Login(login);
            return Json(BaseResponse<bool>.BadRequest(res));
        }
        #endregion

        #region Utilities
        #region Private

        private IActionResult UnauthorizeError(string action = "", object data = null)
        {
            if (!string.IsNullOrEmpty(action))
                return RedirectToAction("Error", "Home", new { message = $"Unauthorized error 401 Action: {action}" });
            return RedirectToAction("Error", "Home", new { message = "Unauthorized error 401" });
        }
        private bool VerifyDomainId(string domainId)
        {
            try
            {
                if (string.IsNullOrEmpty(domainId))
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Private
        private async Task SignInAsync(AuthenticateViewModel authenticateViewModel)
        {
            var claims = JwtTokenHelper.GetUserClaims(authenticateViewModel);
            claims.Add(new Claim(ClaimConstant.TokenExpired, authenticateViewModel.Expired));
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme)),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(60)
                });
        }
        #endregion

        #endregion
    }
}