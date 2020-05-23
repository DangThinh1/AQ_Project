using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AQConfigurations.Core.Services.Implements;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Models.AppSettings;
using AQConfigurations.Core.Helper;
using System.Threading;

namespace AQConfigurations.Core.Extensions
{
    public static class ConfigurationsAppSettingExtension
    {
        public static T Value<T>(this EnvironmentOptionBase<T> option)
        {
            var appSettingEnvironmentService = ConfigurationsInjectionHelper.GetRequiredService<IAppSettingEnvironmentService>();
            var environment = appSettingEnvironmentService.Enviroment;
            switch (environment)
            {
                case 0:
                    return option.LOCAL;
                case 1:
                    return option.VN;
                case 2:
                    return option.BETA;
                case 3:
                    return option.LIVE;
                default: throw new Exception("Please check environment variable in appsetting.json");
            }
        }

        public static IServiceCollection WaitForExecute(this IServiceCollection services, int millisecond = 500)
        {
            var hostingEnvironment = services.BuildServiceProvider().GetRequiredService<IHostingEnvironment>();
            if(hostingEnvironment.IsDevelopment())
                Thread.Sleep(millisecond);
            return services;
        }

        public static IServiceCollection AddAppSettingConnectionString(this IServiceCollection services, string database, string user = null, string password = null, bool syncDevelopment = true, string binFolderPath = null)
        {
            services.TryAddRequiredService(binFolderPath);
            var _configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = new ConnectionString(database, user,password);
            var optionName = typeof(ConnectionString).Name;
            services.AddAppSettingValue(optionName, connectionString);
            services.Configure<ConnectionString>(_configuration.GetSection(optionName));
            return services; 
        }

        public static IServiceCollection AddAppSettingBase(this IServiceCollection services, bool syncDevelopment = false, int value = 0, string binFolderPath = null)
        {
            services.TryAddRequiredService(binFolderPath);
            services.CreateEnvironment(0, syncDevelopment);
            return services;
        }

        public static IServiceCollection AddAppSettingValue<T>(this IServiceCollection services, bool syncDevelopment = false, string binFolderPath = null) where T : class, new()
        {
            var _configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var name = typeof(T).Name;
            services.TryAddRequiredService(binFolderPath);
            services.CreateAppSettingValue(name, new T(), syncDevelopment);
            services.AddOptions();
            services.Configure<T>(_configuration.GetSection(name));
            return services;
        }

        public static IServiceCollection AddAppSettingValue(this IServiceCollection services, string name, object value, bool syncDevelopment = false, string binFolderPath = null)
        {
            services.TryAddRequiredService(binFolderPath);
            services.CreateAppSettingValue(name, value, syncDevelopment);
            return services;
        }

        #region Private

        private static IServiceCollection CreateEnvironment(this IServiceCollection services, int environment = 0, bool syncDevelopment = false)
        {
            var appConfiguration = services.BuildServiceProvider().GetRequiredService<IAppSettingConfigurationService>();
            var appEnvironment = services.BuildServiceProvider().GetRequiredService<IAppSettingEnvironmentService>();
            appConfiguration.Create(appEnvironment.EnvironmentScheme, environment);
            if (syncDevelopment)
                appConfiguration.CreateDevelopment(appEnvironment.EnvironmentScheme, environment);
            return services;
        }

        private static IServiceCollection CreateAppSettingValue(this IServiceCollection services, string name, object value, bool syncDevelopment = false)
        {
            var hostingEnvironment = services.BuildServiceProvider().GetRequiredService<IHostingEnvironment>();
            if (hostingEnvironment.IsDevelopment() && !string.IsNullOrEmpty(name))
            {
                var appConfiguration = services.BuildServiceProvider().GetRequiredService<IAppSettingConfigurationService>();
                appConfiguration.Create(name, value);
                if (syncDevelopment)
                    appConfiguration.CreateDevelopment(name, value);
            }
            return services;
        }

        private static bool IsExistedService<T>(this IServiceCollection services)
        {
            try
            {
                var provider = services.BuildServiceProvider();
                var service = provider.GetService<T>();
                return service != null;
            }
            catch
            {
                return false;
            }
        }

        private static IServiceCollection TryAddRequiredService(this IServiceCollection services, string binFolderPath)
        {
            services.TryAppSettingConfigurationService(binFolderPath);
            services.TryAddAppSettingEnvironmentService();
            ConfigurationsInjectionHelper.TryInit(ref services);
            return services;
        }

        private static IServiceCollection TryAddAppSettingEnvironmentService(this IServiceCollection services, string name = null)
        {
            if (!services.IsExistedService<IAppSettingEnvironmentService>())
            {
                services.AddSingleton<IAppSettingEnvironmentService, AppSettingEnvironmentService>(sp => {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    if (!string.IsNullOrEmpty(name))
                        return new AppSettingEnvironmentService(configuration, name);
                    return new AppSettingEnvironmentService(configuration);
                });
                var appSettingEnvironmentService = services.BuildServiceProvider().GetRequiredService<IAppSettingEnvironmentService>();
            }
            return services;
        }

        private static IServiceCollection TryAppSettingConfigurationService(this IServiceCollection services, string binFolderPath = null)
        {
            if (!services.IsExistedService<IAppSettingConfigurationService>())
            {
                services.AddSingleton<IAppSettingConfigurationService, AppSettingConfigurationService>(sp => {
                    var configuration = sp.GetRequiredService<IConfiguration>();
                    var hostingEnvironment = sp.GetRequiredService<IHostingEnvironment>();
                    if (!string.IsNullOrEmpty(binFolderPath))
                        return new AppSettingConfigurationService(configuration, hostingEnvironment, binFolderPath);
                    return new AppSettingConfigurationService(configuration, hostingEnvironment);
                });
            }
            return services;
        }

        #endregion Private
    }
} 