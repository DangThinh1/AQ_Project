using Identity.Core.Helpers;
using Identity.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Identity.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Core.Authorize
{
    public class IdentitySSOAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    { 
        private const string DEFAULT_ACTION_NAME = "Login";
        private const string DEFAULT_CONTROLLER_NAME = "User";
        private const string DEFAULT_AUTH_SCHEME = ".AQB";

        private string _loginActionName { get; set; }
        private string _loginControllerName { get; set; }
        public string _authenticationScheme { get; set; }
        private readonly ISignInRequestService _signInService;
        private readonly IAuthenticateRequestService _authenticateRequestService;

        public IdentitySSOAuthorizeAttribute()
        {
            _loginActionName = DEFAULT_ACTION_NAME;
            _authenticationScheme = DEFAULT_AUTH_SCHEME;
            _loginControllerName = DEFAULT_CONTROLLER_NAME;
            _signInService = IdentityInjectionHelper.GetService<ISignInRequestService>();
            _authenticateRequestService = IdentityInjectionHelper.GetService<IAuthenticateRequestService>();
        }

        public IdentitySSOAuthorizeAttribute(IAuthenticateRequestService authenticateRequestService = null, ISignInRequestService signInService = null)
        {
            _loginActionName = DEFAULT_ACTION_NAME;
            _authenticationScheme = DEFAULT_AUTH_SCHEME;
            _loginControllerName = DEFAULT_CONTROLLER_NAME;
            _signInService = signInService != null
                ? signInService
                : IdentityInjectionHelper.GetService<ISignInRequestService>();
            _authenticateRequestService = authenticateRequestService != null
                ? authenticateRequestService
                : IdentityInjectionHelper.GetService<IAuthenticateRequestService>();
        }

        public IdentitySSOAuthorizeAttribute(string controllerName, string actionName, string authenticationScheme, IAuthenticateRequestService authenticateRequestService = null, ISignInRequestService signInService = null)
        {
            _loginActionName = actionName;
            _loginControllerName = controllerName;
            _authenticationScheme = string.IsNullOrEmpty(authenticationScheme) ? DEFAULT_AUTH_SCHEME : authenticationScheme;
            _signInService = signInService != null 
                ? signInService
                :IdentityInjectionHelper.GetService<ISignInRequestService>();
            _authenticateRequestService = authenticateRequestService != null 
                ? authenticateRequestService 
                : IdentityInjectionHelper.GetService<IAuthenticateRequestService>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;

            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
                BackToLoginPage(context);

            var token = user.GetUserToken();
            if (string.IsNullOrEmpty(token) || !_signInService.IsAllowedToken(token).IsSuccessStatusCode)
            {
                var result = _signInService.SignOutAllDevicesAsync(context.HttpContext, _authenticationScheme).Result;
                BackToLoginPage(context);
            }
        }

        private void BackToLoginPage(AuthorizationFilterContext context)
        {
            string url = context.HttpContext.Request.Path;
            context.Result = new RedirectToActionResult(_loginActionName, _loginControllerName, new { returnUrl = url });
            return;
        }
    }
}
