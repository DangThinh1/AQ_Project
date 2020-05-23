using APIHelpers.ServerHost;
using Microsoft.Extensions.DependencyInjection;
using AQConfigurations.Core.Services.Implements;
using AQConfigurations.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using AQConfigurations.Core.Helper;

namespace AQConfigurations.Core.Extensions
{
    public static class ConfigurationsRequestServiceExtension 
    {
        public static IServiceCollection AddConfigurationsRequestService(this IServiceCollection services, string apiServer)
        {
            ConfigurationsInjectionHelper.TryInit(ref services);
            ServerHostHelper.AddServerHostByName(ServerHostNameConts.ConfigurationApi, apiServer);
            #region Base
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion Base

            #region Location Services
            services.TryAddScoped<ILocationRequestService, LocationRequestService>();
            services.TryAddScoped<ICityRequestService, CityRequestService>();
            services.TryAddScoped<ICountryRequestService, CountryRequestService>();
            services.TryAddScoped<IZoneDistrictRequestService, ZoneDistrictRequestService>();
            #endregion Location Services

            services.TryAddScoped<ICurrencyRequestService, CurrencyRequestService>();
            services.TryAddScoped<ICommonValueRequestService, CommonValueRequestService>();
            services.TryAddScoped<ICommonResourceRequestService, CommonResourceRequestService>();
            services.TryAddScoped<IPortalLanguageRequestService, PortalLanguageRequestService>();
            services.TryAddScoped<IPortalLocationRequestService, PortalLocationRequestService>();
            services.TryAddScoped<ICommonLanguagesRequestServices, CommonLanguagesRequestServices>();
            services.TryAddScoped<IMultiLanguageService, MultiLanguageService>();
            
            return services;
        }
    }
}