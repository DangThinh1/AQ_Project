using Identity.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Identity.Core.Portal.Services.Implements;
using Identity.Core.Portal.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Identity.Core.Portal.Extensions
{
    public static class PortalIdentityWebServiceExtension
    {
        public static void AddPortalIdentityWebService(this IServiceCollection services, IConfiguration configuration, string identityServerUrl = "")
        {
            services.AddIdentityWebService(configuration, identityServerUrl);
            services.TryAddScoped<ISSOAuthenticationRequestService, SSOAuthenticationRequestService>();
        }
    }
}