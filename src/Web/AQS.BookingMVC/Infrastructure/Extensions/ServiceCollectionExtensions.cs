using AQS.BookingMVC.Infrastructure.ConfigModel;
using AQS.BookingMVC.Infrastructure.EmailSender;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Models.Config;
using Identity.Core.Common;
using Identity.Core.Portal.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.WebEncoders;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using DependencyInjectionHelper = AQS.BookingMVC.Infrastructure.Helpers.DependencyInjectionHelper;

namespace AQS.BookingMVC.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string API_CONFIG_SECTION = "ApiServer";
        private const string YATCH_URL_CONFIG_SECTION = "YachtPortalApiUrl";
        private const string BASE_URL_CONFIG_SECTION = "BaseApiUrl";
        private const string SETTING = "Setting";
        private const string ADMIN_URL_CONFIG_SECTION = "AdminApiUrl";
        private const string EMAIL_SETTING_CONFIG_SECTION = "EmailSettings";
        private const string COMMON_SETTINg_CONFIG_SECTION = "CommonSettings";
        private const string FILESTREAM_URL_CONFIG_SECTION = "FileStreamApiUrl";
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // load config to model here
            services.Configure<ApiServer>(configuration.GetSection(API_CONFIG_SECTION));
            services.Configure<YachtPortalApiUrl>(configuration.GetSection(YATCH_URL_CONFIG_SECTION));
            services.Configure<BaseApiUrl>(configuration.GetSection(BASE_URL_CONFIG_SECTION));
            services.Configure<Setting>(configuration.GetSection(SETTING));
            services.Configure<AdminApiUrl>(configuration.GetSection(ADMIN_URL_CONFIG_SECTION));
            services.Configure<EmailSettings>(configuration.GetSection(EMAIL_SETTING_CONFIG_SECTION));
            services.Configure<CommonSettings>(configuration.GetSection(COMMON_SETTINg_CONFIG_SECTION));
            services.Configure<FileStreamApiUrl>(configuration.GetSection(FILESTREAM_URL_CONFIG_SECTION));
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
            return services;
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = CookiesAuthenticationDomainConstant.AQDiningPortal; // Aq booking shared cookie , Require same name for all service use with the same account system
                options.Cookie.Domain = GetCookieDomain();
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                //options.DataProtectionProvider = _provider;
                //options.TicketDataFormat = new TicketDataFormat(_provider.CreateProtector(SecurityConstant.AQSecurityMasterProtector));
                options.Events.OnValidatePrincipal = PrincipalSecurityStampValidator.ValidatePrincipalAsync;
            });
            services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = CookiesAuthenticationDomainConstant.AQDiningPortal; // Aq booking shared cookie , Require same name for all service use with the same account system
                options.Cookie.Domain = GetCookieDomain();
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                //options.DataProtectionProvider = _provider;
                //options.TicketDataFormat = new TicketDataFormat(_provider.CreateProtector(SecurityConstant.AQSecurityMasterProtector));
                options.Events.OnValidatePrincipal = PrincipalSecurityStampValidator.ValidatePrincipalAsync;
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
        private static string GetCookieDomain()
        {
            var baseDomain = ApiUrlHelper.AQBaseDomainPortal;
            return baseDomain ?? string.Empty;
        }
    }
}
