using APIHelpers.ServerHost;
using Microsoft.Extensions.DependencyInjection;
using AQConfigurations.Core.Services.Implements;
using AQConfigurations.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AQConfigurations.Core.Extensions
{
    public static class FileStreamRequestServiceExtension
    {
        public static IServiceCollection AddFileStreamsRequestService(this IServiceCollection services, string apiServer)
        {
            ServerHostHelper.AddServerHostByName("FileStreamApi", apiServer);
            services.TryAddScoped<IFileStreamRequestService, FileStreamRequestService>();
            return services;
        }
    }
}