using Identity.Core.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Core.Portal.Helpers
{
    public  class PrincipalValidator: ISecurityStampValidator
    {
        /// <summary>
        /// The <see cref="ISystemClock"/>.
        /// </summary>
        public  ISystemClock Clock { get; }
        public  SecurityStampValidatorOptions Options { get; }
        public PrincipalValidator(IOptions<SecurityStampValidatorOptions> options, ISystemClock clock)
        {
            Options = options.Value;
            Clock = clock;
        }
        public  async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            if (context == null) throw new System.ArgumentNullException(nameof(context));

            var currentUtc = DateTimeOffset.UtcNow;
            if (context.Options != null && Clock != null)
            {
                currentUtc = Clock.UtcNow;
            }
            var issuedUtc = context.Properties.IssuedUtc;

            // Only validate if enough time has elapsed
            var validate = (issuedUtc == null);
            if (issuedUtc != null)
            {
                var timeElapsed = currentUtc.Subtract(issuedUtc.Value);
                validate = timeElapsed > Options.ValidationInterval;
            }

            if (validate)
            {
                var userId = context.Principal.Claims.FirstOrDefault(claim => claim.Type == ClaimConstant.UserId)?.Value;
                // Get an instance using DI
                var accessToken = context.Principal.Claims.FirstOrDefault(claim => claim.Type == ClaimConstant.AccessToken)?.Value;
                if (userId != null && accessToken != null)
                {
                    await SecurityStampVerified(context);
                    return;
                }
                else
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return;
                }
            }
        }


        public  async Task SecurityStampVerified (CookieValidatePrincipalContext context)
        {
            var newPrincipal = new ClaimsPrincipal(new ClaimsIdentity(context.Principal?.Claims, CookieAuthenticationDefaults.AuthenticationScheme));

            if (Options.OnRefreshingPrincipal != null)
            {
                var replaceContext = new SecurityStampRefreshingPrincipalContext
                {
                    CurrentPrincipal = context.Principal,
                    NewPrincipal = newPrincipal
                };

                // Note: a null principal is allowed and results in a failed authentication.
                await Options.OnRefreshingPrincipal(replaceContext);
                newPrincipal = replaceContext.NewPrincipal;
            }

            // REVIEW: note we lost login authentication method
            context.ReplacePrincipal(newPrincipal);
            context.ShouldRenew = true;
        }
    }

    public static class PrincipalStampValidator 
    {
        /// <summary>
        /// The <see cref="ISystemClock"/>.
        /// </summary>
        public static ISystemClock Clock { get; }
        public static SecurityStampValidatorOptions Options { get; }
       
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            if (context == null) throw new System.ArgumentNullException(nameof(context));

            var currentUtc = DateTimeOffset.UtcNow;
            if (context.Options != null && Clock != null)
            {
                currentUtc = Clock.UtcNow;
            }
            var issuedUtc = context.Properties.IssuedUtc;

            // Only validate if enough time has elapsed
            var validate = (issuedUtc == null);
            if (issuedUtc != null)
            {
                var timeElapsed = currentUtc.Subtract(issuedUtc.Value);
                validate = timeElapsed > Options.ValidationInterval;
            }

            if (validate)
            {
                var userId = context.Principal.Claims.FirstOrDefault(claim => claim.Type == ClaimConstant.UserId)?.Value;
                // Get an instance using DI
                var accessToken = context.Principal.Claims.FirstOrDefault(claim => claim.Type == ClaimConstant.AccessToken)?.Value;
                if (userId != null && accessToken != null)
                {
                    await SecurityStampVerified(context);
                    return;
                }
                else
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return;
                }
            }
        }


        public static async Task SecurityStampVerified(CookieValidatePrincipalContext context)
        {
            var newPrincipal = new ClaimsPrincipal(new ClaimsIdentity(context.Principal?.Claims, CookieAuthenticationDefaults.AuthenticationScheme));

            if (Options.OnRefreshingPrincipal != null)
            {
                var replaceContext = new SecurityStampRefreshingPrincipalContext
                {
                    CurrentPrincipal = context.Principal,
                    NewPrincipal = newPrincipal
                };

                // Note: a null principal is allowed and results in a failed authentication.
                await Options.OnRefreshingPrincipal(replaceContext);
                newPrincipal = replaceContext.NewPrincipal;
            }

            // REVIEW: note we lost login authentication method
            context.ReplacePrincipal(newPrincipal);
            context.ShouldRenew = true;
        }
    }


    /// <summary>
    /// Static helper class used to configure a CookieAuthenticationNotifications to validate a cookie against a user's security
    /// stamp.
    /// </summary>
    public static class PrincipalSecurityStampValidator
    {
        /// <summary>
        /// Validates a principal against a user's stored security stamp.
        /// </summary>
        /// <param name="context">The context containing the <see cref="System.Security.Claims.ClaimsPrincipal"/>
        /// and <see cref="AuthenticationProperties"/> to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous validation operation.</returns>
        public static Task ValidatePrincipalAsync(CookieValidatePrincipalContext context)
            => ValidateAsync<PrincipalValidator>(context);

        /// <summary>
        /// Used to validate the <see cref="IdentityConstants.TwoFactorUserIdScheme"/> and 
        /// <see cref="IdentityConstants.TwoFactorRememberMeScheme"/> cookies against the user's 
        /// stored security stamp.
        /// </summary>
        /// <param name="context">The context containing the <see cref="System.Security.Claims.ClaimsPrincipal"/>
        /// and <see cref="AuthenticationProperties"/> to validate.</param>
        /// <returns></returns>

        public static Task ValidateAsync<TValidator>(CookieValidatePrincipalContext context) where TValidator : PrincipalValidator
        {
            if (context.HttpContext.RequestServices == null)
            {
                throw new InvalidOperationException("RequestServices is null.");
            }

            var validator = context.HttpContext.RequestServices.GetRequiredService<TValidator>();
            return validator.ValidateAsync(context);
        }
    }
}
