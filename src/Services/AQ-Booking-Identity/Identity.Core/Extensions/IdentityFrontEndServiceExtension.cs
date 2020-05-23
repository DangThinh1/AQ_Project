using Identity.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Identity.Core.Services.Implements;
using Identity.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Identity.Core.Extensions 
{
    public static class IdentityFrontEndServiceExtension
    {
        [Obsolete("Please use AddIdentityWebService instead")]
        public static void AddIdentityFrontEndService(this IServiceCollection services, IConfiguration configuration, string identityServerUrl="")
        {
            IdentityInjectionHelper.Init(ref services);
            IdentityInjectionHelper.SetBaseUrl(identityServerUrl);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUsersContext, UsersContext>();
            services.TryAddScoped<ISignInRequestService, SignInRequestService>();
            services.TryAddScoped<IRolesRequestServices, RolesRequestServices>();
            services.TryAddScoped<IAccountsRequestService, AccountsRequestService>();
            services.TryAddScoped<IAuthenticateRequestService, AuthenticateRequestService>();
        }
    }
}