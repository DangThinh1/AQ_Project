using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace YachtMerchant.Core.Helpers.Authentication
{
    public class CustomizeAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private string _loginActionName { get; set; }
        private string _loginControllerName { get; set; }

        public CustomizeAuthorizeAttribute()
        {
            _loginActionName = "Login";
            _loginControllerName = "User";
        }

        public CustomizeAuthorizeAttribute(string controllerName, string actionName)
        {
            _loginActionName = actionName;
            _loginControllerName = controllerName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)

        {
            var user = context.HttpContext.User;

            if (user.Identity.IsAuthenticated == false)
            {
                string url = context.HttpContext.Request.Path;
                context.Result = new RedirectToActionResult(_loginActionName, _loginControllerName, new { returnUrl = url });
                return;
            }
        }
    }
}
