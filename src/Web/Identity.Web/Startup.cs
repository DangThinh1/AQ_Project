using System;
using Identity.Web.AppConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Identity.Core.Portal.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Identity.Core.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Identity.Web.Helpers;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.StackExchangeRedis;
using Identity.Core.Portal.Helpers;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace Identity.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOption(Configuration);
            services.AddSSOPortalService();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //**********Note: Please don't remove this line or move to anywhere*****************
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //***************Data protection master key***********************
            var protectionProvider = Helpers.PersistentProviderHelper.CreateRedisProvider(services);
            var redisConnection = RedisCacheHelper.ConnectToRedisServer(ApiUrlHelper.RedisCacheSrv.Host, ApiUrlHelper.RedisCacheSrv.Port);
            services.Configure<KeyManagementOptions>(opt =>
            {
                opt.XmlRepository = new RedisXmlRepository(() => redisConnection.GetDatabase(), SecurityConstant.AQSecurityMasterKeyRedis);
                opt.AutoGenerateKeys = true; //*********Master Redis Balancer: must stast first before run other project**********
            });

            services.Configure<SecurityStampValidatorOptions>(options =>
               options.ValidationInterval = TimeSpan.FromMinutes(15)
            );

            //**********Note: Please don't remove this line or move to anywhere*****************
            var _provider = DependencyInjectionHelper.GetService<IDataProtectionProvider>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = CookiesAuthenticationDomainConstant.AQSSOPortal; //Aq booking shared cookie,Require same name for all service use with the same account system
                options.Cookie.Domain = GetCookieDomain();
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                options.Cookie.Expiration = TimeSpan.FromDays(60);
                options.DataProtectionProvider = _provider;
                options.TicketDataFormat = new TicketDataFormat(_provider.CreateProtector(SecurityConstant.AQSecurityMasterProtector));
                options.Events.OnValidatePrincipal = PrincipalSecurityStampValidator.ValidatePrincipalAsync;
            });
            services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = CookiesAuthenticationDomainConstant.AQSSOPortal; // Aq booking shared cookie , Require same name for all service use with the same account system
                options.Cookie.Domain = GetCookieDomain();
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                options.Cookie.Expiration = TimeSpan.FromDays(60);
                options.DataProtectionProvider = _provider;
                options.TicketDataFormat = new TicketDataFormat(_provider.CreateProtector(SecurityConstant.AQSecurityMasterProtector));
                options.Events.OnValidatePrincipal = PrincipalSecurityStampValidator.ValidatePrincipalAsync;
            });

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = $"{ApiUrlHelper.RedisCacheSrv.Host}:{ApiUrlHelper.RedisCacheSrv.Port}";
                option.InstanceName = ApiUrlHelper.RedisCacheSrv.InstanceName;
            });
            services.AddSession(option =>
            {
                option.Cookie.SameSite = SameSiteMode.Lax;
                option.IdleTimeout = TimeSpan.FromMinutes(20);
                option.Cookie.Name = $"{CookiesAuthenticationDomainConstant.AQSSOPortal}_PK";
                option.Cookie.IsEssential = true;
            });

           
            services.AddHttpContextAccessor();
            services.AddPortalIdentityWebService(Configuration, ApiUrlHelper.IdentityApiUrl);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });



            //************Please don't change this live to anywhere (tO@N) **************
            app.UseCookiePolicy();
        }

        /// <summary>
        ///  Get Cookies Base Domain
        /// </summary>
        /// <returns></returns>
        private string GetCookieDomain()
        {
            var baseDomain = ApiUrlHelper.AQBaseDomainPortal;
            return baseDomain ?? string.Empty;
        }
    }

    public static class ServiceCollectionExtension
    {
        public static void AddSSOPortalService(this IServiceCollection services)
        {
            services.TryAddScoped<PrincipalValidator>();
            services.TryAddSingleton<RedisXmlRepository> ();

        }

        public static void AddOption(this IServiceCollection services,IConfiguration configuration)
        {
            DependencyInjectionHelper.Init(ref services);
            services.AddOptions();
            services.Configure<ApiServer>(configuration.GetSection("ApiServer"));
        }
    }
}