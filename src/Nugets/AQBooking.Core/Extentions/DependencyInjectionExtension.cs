using AQBooking.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace AQBooking.Core.Extentions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            AQDependencyInjectionHelper.TryInit(ref services);
            return services;
        }
    }
}