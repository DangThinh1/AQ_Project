using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Helpers;
using AQBooking.Admin.Core.Models.AuthModel;
using AQS.BookingAdmin.Services.Interfaces.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Controllers
{
    public class AccountController: BaseController
    {

        #region Fields
        private readonly IAuthService _authService;
        #endregion

        #region Ctor
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        #endregion

        #region Methods
        #region Actions
        #region Login / Logout
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            await HttpContext.SignOutAsync();

            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginModel();
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.AuthorizeUser(model);
                if (result.AccessToken != null)
                {
                    if (result.RoleId != (int)UserRoleEnum.DiningMerchant && result.RoleId != (int)UserRoleEnum.YachtMerchant)
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, result.Email),
                            new Claim(ClaimTypes.Rsa, result.AccessToken),
                            new Claim(ClaimTypes.NameIdentifier, result.UserId),
                            new Claim(ClaimTypes.Role, result.UserRole)
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = WebHelper.ConvertUnixTimeToDate(result.Expired)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                       // await _authService.UpdateUserLang(result.UserId, model.Language ?? 1, result.AccessToken);

                        if (!string.IsNullOrWhiteSpace(returnUrl))
                            return Redirect(returnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Access is denied";
                        return View("Login", model);
                    }
                }
                else
                {
                    ViewBag.Error= "User not valid: " + result.Message;
                   
                    return View("Login", model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        #endregion
        #endregion
        #region Utilities

        #endregion
        #endregion


    }
}
