using Identity.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Identity.Core.Services.Implements;
using Identity.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Identity.Core.Extensions
{
    public static class IdentityWebServiceExtension
    {
        public static void AddIdentityWebService(this IServiceCollection services, IConfiguration configuration, string identityServerUrl = null)
        {
            ConfigureServices(services);
            IdentityInjectionHelper.SetBaseUrl(identityServerUrl);
        }

        public static void AddIdentityWebService(this IServiceCollection services, string identityServerUrl = null)
        {
            ConfigureServices(services);
            IdentityInjectionHelper.SetBaseUrl(identityServerUrl);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            IdentityInjectionHelper.Init(ref services);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUsersContext, UsersContext>();
            services.TryAddScoped<ISignInRequestService, SignInRequestService>();
            services.TryAddScoped<IRolesRequestServices, RolesRequestServices>();
            services.TryAddScoped<IAccountsRequestService, AccountsRequestService>();
            services.TryAddScoped<IAuthenticateRequestService, AuthenticateRequestService>();
        }
    }
}