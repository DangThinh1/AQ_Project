using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Core.Authorize
{
    class IdentityAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private string _loginActionName { get; set; }
        private string _loginControllerName { get; set; }

        public IdentityAuthorizeAttribute()
        {
            _loginActionName = "Login";
            _loginControllerName = "User";
        }

        public IdentityAuthorizeAttribute(string controllerName, string actionName)
        {
            _loginActionName = actionName;
            _loginControllerName = controllerName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            string url = context.HttpContext.Request.Path;
            if (user.Identity.IsAuthenticated == false)
            {
                context.Result = new RedirectToActionResult(_loginActionName, _loginControllerName, new { returnUrl = url });
                return;
            }
        }
    }
} 
