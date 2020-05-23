using Identity.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Identity.Core.Helpers;
using Identity.Core.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Identity.Web.Helpers;
using System.Linq;
using Identity.Core.Portal.Conts;
using Identity.Web.AppConfig;
using Identity.Core.Models.Authentications;
using Microsoft.AspNetCore.Http;
using Identity.Core.Portal.Services.Interfaces;
using Identity.Core.Portal.Models.SSOAuthentication;
using Microsoft.AspNetCore.DataProtection;
using System.Text;
using Identity.Core.Common;

namespace Identity.Web.Controllers
{
    public class UserController : Controller
    {
        private const string THIS_DOMAIN_ID = SSOConts.SSO_PORTAL_DOMAIN_ID;
        private ISignInRequestService _signInService;
        private IAccountsRequestService _accountsRequestService;
        private IAuthenticateRequestService _authenticateRequestService;
        private ISSOAuthenticationRequestService _sSOAuthenticationRequestService;
        private readonly IDataProtector _dataProtector;
        public UserController(ISignInRequestService signInService,
                              IAccountsRequestService accountsRequestService,
                              IAuthenticateRequestService authenticateRequestService,
                              ISSOAuthenticationRequestService sSOAuthenticationRequestService,
                              IDataProtectionProvider dataProtectorProvider)
        {
            _signInService = signInService;
            _accountsRequestService = accountsRequestService;
            _authenticateRequestService = authenticateRequestService;
            _sSOAuthenticationRequestService = sSOAuthenticationRequestService;
            _dataProtector = dataProtectorProvider.CreateProtector(SecurityConstant.AQSecurityMasterProtector);
        }

        [HttpGet]
        public IActionResult TestProtect(string word)
        {
            var protectedString = _dataProtector.Protect(word);
            var unprotectedString = _dataProtector.Unprotect(protectedString);

            return Json($"Encrypted string:{protectedString}, Decrypted string:{unprotectedString}");
        }

        [HttpGet]
        public IActionResult TestUnProtect(string key)
        {
            try
            {
                var unprotectedString = _dataProtector.Unprotect(key);

                return Json($"Key Encrypted:{key}, Decrypted string:{unprotectedString}");
            }
            catch(Exception ex)
            {
                return Json($"Key Encrypted:{key}, Decrypted string: invalid key, Detail:{ex.Message}");
            }
        }

        public IActionResult Index(string d)
        {
            if (!VerifyDomainId(d))
                return UnauthorizeError("Index");
            return RedirectToAction("LoginStart", new { d });
        }

        #region Login

        public IActionResult LoginStart(string d)
        {
            if (!VerifyDomainId(d))
                return UnauthorizeError("LoginStart");
            Response.Cookies.Append(SSOConts.REDIRECT_DOMAIN_ID_COOKIE_NAME, d);
            return RedirectToAction("Login");
        }

        public IActionResult LoginNext()//redicrect to next domain
        {
            var accessToken = UserContextHelper.AccessToken;
            var userUid = UserContextHelper.UserUniqueId;
            var ssoAuthCreateModels = SSOControlHelper.SigninFlows.Select(k => new SSOAuthenticationCreateModel()
            {
                Token = accessToken,
                UserUid = userUid,
                DomainId = k.DomainId,
                RedirectUrl = k.RedirectUrl,
                Type = true
                
            }).ToList();
            var clearResponse = _sSOAuthenticationRequestService.Clear(userUid);
            var createLoginSessionResponse = _sSOAuthenticationRequestService.Create(ssoAuthCreateModels);
            if(createLoginSessionResponse.IsSuccessStatusCode)
                return LoginNextDomain(createLoginSessionResponse.ResponseData);
            return UnauthorizeError("LoginNext");
        }

        public IActionResult LoginEnd(string i)//redicrect to domain wich first call to SSO
        {
            var callBackdomainId = HttpContext.Request.Cookies[SSOConts.REDIRECT_DOMAIN_ID_COOKIE_NAME]?.ToString();
            var callBackDomain = ApiUrlHelper.SigninControls.Portals.FirstOrDefault(k=> k.DomainId == callBackdomainId);
            DeleteAuthData();
            if (callBackDomain != null)
                return Redirect($"{callBackDomain.Host}{callBackDomain.LoginEndPath}");
            return UnauthorizeError("LoginEnd");
        }

        public IActionResult Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("LoginNext");

            if (!string.IsNullOrEmpty(model.Email))
            {
                var response = _accountsRequestService.VerifyEmailForSignIn(model.Email);
                model.IsValidEmail = response.IsSuccessStatusCode;
                model.LoginMessage = response.Message;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string password)
        {
            if (model == null || string.IsNullOrEmpty(password))
                return View(model);
            if (!User.Identity.IsAuthenticated)
            {
                var authResult = _authenticateRequestService.Authenticate(model.Email, password);
                if (authResult.IsSuccessStatusCode)
                {
                    await LoginAsync(authResult.ResponseData);
                    return RedirectToAction("LoginNext");
                }
            }
            return View(model);
        }

        private async Task LoginAsync(AuthenticateViewModel authModel)
        {
            var claims = JwtTokenHelper.GetUserClaims(authModel);
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
        private IActionResult LoginNextDomain(string authId)
        {
            var ssoAuthResponse = _sSOAuthenticationRequestService.FindByDomainId(authId, THIS_DOMAIN_ID);
            if(ssoAuthResponse.IsSuccessStatusCode)
            {
                var redirectUrl = $"{ssoAuthResponse.ResponseData?.RedirectUrl}?i={authId}" ;
                return Redirect(redirectUrl);
            }
            return UnauthorizeError("LoginNextDomain");
        }
        #endregion Login

        #region Logout

        public IActionResult Logout(string d)
        {
            if (!VerifyDomainId(d))
                return UnauthorizeError("Logout");
            HttpContext.Response.Cookies.Append(SSOConts.REDIRECT_DOMAIN_ID_COOKIE_NAME, d);
            return RedirectToAction("LogoutNext");
        }

        public async Task<IActionResult> LogoutNext()
        {
            var accessToken = UserContextHelper.AccessToken;
            var userUid = UserContextHelper.UserUniqueId;
            var ssoAuthCreateModels = SSOControlHelper.SignoutFlows.Select(k => new SSOAuthenticationCreateModel()
            {
                Token = accessToken,
                UserUid = userUid,
                DomainId = k.DomainId,
                RedirectUrl = k.RedirectUrl,
                Type = false
            }).ToList();
            var clearResponse = _sSOAuthenticationRequestService.Clear(userUid);
            var createLogoutSessionResponse = _sSOAuthenticationRequestService.Create(ssoAuthCreateModels);
            if(createLogoutSessionResponse.IsSuccessStatusCode)
            {
                await LogoutAsync();
                return LogoutNextDomain(createLogoutSessionResponse.ResponseData);
            }
            return UnauthorizeError("LogoutNext");
        }

        public IActionResult LogoutEnd()//redicrect to domain wich first call to SSO
        {
            var callBackdomainId = HttpContext.Request.Cookies[SSOConts.REDIRECT_DOMAIN_ID_COOKIE_NAME]?.ToString();
            var callBackDomain = ApiUrlHelper.SigninControls.Portals.FirstOrDefault(k => k.DomainId == callBackdomainId);
            DeleteAuthData();
            if (callBackDomain != null)
                return Redirect($"{callBackDomain.Host}{callBackDomain.LogoutEndPath}");
            return UnauthorizeError("LogoutEnd");
        }

        private IActionResult LogoutNextDomain(string authId)
        {
            var ssoAuthResponse = _sSOAuthenticationRequestService.FindByDomainId(authId, THIS_DOMAIN_ID);
            if (ssoAuthResponse.IsSuccessStatusCode)
            {
                var redirectUrl = $"{ssoAuthResponse.ResponseData?.RedirectUrl}?i={authId}";
                return Redirect(redirectUrl);
            }
            return UnauthorizeError("LogoutNextDomain");
        }
        private async Task LogoutAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
        #endregion Logout

        #region Private

        private IActionResult UnauthorizeError(string action="")
        {
            if(!string.IsNullOrEmpty(action))
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
        private void DeleteAuthData()
        {
            var userUid = UserContextHelper.UserUniqueId;
            var clearResponse = _sSOAuthenticationRequestService.Clear(userUid);
            HttpContext.Response.Cookies.Delete(SSOConts.REDIRECT_DOMAIN_ID_COOKIE_NAME);
        }

        #endregion Private
    }
}