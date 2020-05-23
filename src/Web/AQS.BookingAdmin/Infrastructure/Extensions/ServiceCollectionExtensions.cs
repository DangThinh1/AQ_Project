using AQConfigurations.Core.Extensions;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace AQS.BookingAdmin.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string API_CONFIG_SECTION = "ApiServer";
        private const string ADMIN_PORTAL_API_URL = "AdminPortalApiUrl";
        private const string WEB_SETTING_CONFIG_SECTION = "WebSetting";
        private const string COMMON_SETTING_CONFIG_SECTION = "CommonSettings";
        
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // load config to model here
            services.Configure<ApiServer>(configuration.GetSection(API_CONFIG_SECTION));
            services.Configure<AdminPortalApiUrl>(configuration.GetSection(ADMIN_PORTAL_API_URL));
            services.Configure<WebSetting>(configuration.GetSection(WEB_SETTING_CONFIG_SECTION));
            services.Configure<CommonSettings>(configuration.GetSection(COMMON_SETTING_CONFIG_SECTION));
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
            
            return services;
        }

        public static IServiceCollection AddSessionAQS(this IServiceCollection services)
        {
            #region Config SESSSION
            //services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                //set timeout for Sesssion
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;//make session cookie essential
            });
            return services;
            #endregion
        }

        public static void AddAQConfigurationsService(this IServiceCollection services, IConfiguration configuration)
        {
            var apiServer = configuration.GetSection(API_CONFIG_SECTION).Get<ApiServer>();
            string apiConfigurationServer = apiServer.AQConfigurationApi.GetCurrentServer(apiServer.Server);
            services.AddConfigurationsRequestService(apiConfigurationServer);
        }
        public static IServiceCollection AddAQSAuthentication(this IServiceCollection services)
        {
            //var protectionProvider = PersistentProviderHelper.CreateRedisProvider(services);
            //var redisConnection = RedisCacheHelper.ConnectToRedisServer(ApiUrlHelper.RedisCacheSrv.Host, ApiUrlHelper.RedisCacheSrv.Port);
            //services.Configure<KeyManagementOptions>(opt =>
            //{
            //    opt.XmlRepository = new RedisXmlRepository(() => redisConnection.GetDatabase(), SecurityConstant.AQSecurityMasterKeyRedis);
            //    opt.AutoGenerateKeys = false; //Second balancer
            //});
            services.Configure<SecurityStampValidatorOptions>(options =>
                 options.ValidationInterval = TimeSpan.FromMinutes(15)
             );
            //**********Note: Please don't remove this line or move to anywhere*****************
           // var _provider = DependencyInjectionHelperPortal.GetService<IDataProtectionProvider>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Account/Login";
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                //options.DataProtectionProvider = _provider;
                //options.TicketDataFormat = new TicketDataFormat(_provider.CreateProtector(SecurityConstant.AQSecurityMasterProtector));
                //options.Events.OnValidatePrincipal = PrincipalSecurityStampValidator.ValidatePrincipalAsync;
            });
            services.ConfigureApplicationCookie(options => {
               
                //options.Cookie.Domain = GetCookieDomain();
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                //options.DataProtectionProvider = _provider;
                //options.TicketDataFormat = new TicketDataFormat(_provider.CreateProtector(SecurityConstant.AQSecurityMasterProtector));
                //options.Events.OnValidatePrincipal = PrincipalSecurityStampValidator.ValidatePrincipalAsync;
            });

            return services;
        }

       
    }
}
