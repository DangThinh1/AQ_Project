using AQBooking.YachtPortal.Web.Interfaces.Common;
using AQConfigurations.Core.Extensions;
using AQConfigurations.Core.Services.Implements;
using AQConfigurations.Core.Services.Interfaces;
using AQS.BookingMVC.Infrastructure.AQPagination;
using AQS.BookingMVC.Infrastructure.EmailSender;
using AQS.BookingMVC.Infrastructure.Extensions;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Infrastructure.Mvc.CustomMiddleware;
using AQS.BookingMVC.Infrastructure.Mvc.Filters;
using AQS.BookingMVC.Interfaces;
using AQS.BookingMVC.Services;
using AQS.BookingMVC.Services.Implements;
using AQS.BookingMVC.Services.Implements.Common;
using AQS.BookingMVC.Services.Implements.Location;
using AQS.BookingMVC.Services.Implements.Post;
using AQS.BookingMVC.Services.Implements.Subscribe;
using AQS.BookingMVC.Services.Implements.UI;
using AQS.BookingMVC.Services.Implements.Yatch;
using AQS.BookingMVC.Services.Interfaces;
using AQS.BookingMVC.Services.Interfaces.Common;
using AQS.BookingMVC.Services.Interfaces.Location;
using AQS.BookingMVC.Services.Interfaces.Post;
using AQS.BookingMVC.Services.Interfaces.Subscribe;
using AQS.BookingMVC.Services.Interfaces.UI;
using AQS.BookingMVC.Services.Interfaces.Yatch;
using Identity.Core.Portal.Extensions;
using Identity.Core.Portal.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AQS.BookingMVC
{

    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return true;
          
        }
    }
    public class Startup
    {
        #region Start Up Config
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        #endregion

        #region Config Services

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDependencyInjectionHelper();
            services.AddSessionAQS();

            ApiUrlHelper.Init(Configuration);
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddAppSettings(Configuration);
            services.AddScoped<LanguguageParamsFilter>();
            services.AddAQSAuthentication();
            ConfigureExternalServices(services);
            ConfigureAQSServices(services);
        }
        #endregion

        #region Config App
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDependencyHelper();           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseLanguageMiddleware();
            
            app.UseEndpoints(endpoints =>
            {
                ConfigAreaRoute(endpoints);
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });


        }
        private void ConfigAreaRoute(IEndpointRouteBuilder endpoints)
        {

            endpoints.MapAreaControllerRoute(
                       name: "DiningArea",
                       areaName: "Dining",
                       pattern: "Dining/{controller=DiningHome}/{action=Index}/{id?}");

            endpoints.MapAreaControllerRoute(
                       name: "HotelArea",
                       areaName: "Hotel",
                       pattern: "Hotel/{controller=HotelHome}/{action=Index}/{id?}");

            endpoints.MapAreaControllerRoute(
                       name: "YachtArea",
                       areaName: "Yacht",
                       pattern: "Yacht/{controller=YachtHome}/{action=Index}/{id?}");

            endpoints.MapAreaControllerRoute(
                       name: "TravelBlogArea",
                       areaName: "TravelBlog",
                       pattern: "{lang:lang}/travel-blog/{controller=TravelBlog}/{action=Index}/{id?}");
        }
        #endregion

        #region Config External Services
        private void ConfigureExternalServices(IServiceCollection services)
        {
            //Register Nuget services here
            services.AddPortalIdentityWebService(Configuration, ApiUrlHelper.IdentityApiUrl);
            services.AddScoped<PrincipalValidator>();
            services.AddConfigurationsRequestService(ApiUrlHelper.ConfigurationApi);
            services.AddScoped<IMultiLanguageService, MultiLanguageService>();
            services.AddScoped<ICommonLanguagesRequestServices, CommonLanguagesRequestServices>();
            services.AddScoped<ICommonValueService, CommonValueService>();
        }
        #endregion

        #region Config AQS Services
        private void ConfigureAQSServices(IServiceCollection services)
        {
            //Register app service here

            #region User
            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<IUserService, UserService>();
            #endregion

            #region Yatch
            services.AddScoped<IYatchService, YatchService>();
            services.AddScoped<IYachtBookingService, YachtBookingService>();
            services.AddScoped<IYachtPaymentService, YachtPaymentService>();
            services.AddScoped<IYachtCharteringService, YachtCharteringService>();
            #endregion

            #region Location
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IPortalLocationService, PortalLocationService>();
            services.AddScoped<IPortLocationService, PortLocationService>();
            #endregion

            #region Travel Blog
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ISubscribeService, SubscribeService>();
            #endregion

            #region Common
            services.AddScoped<IWebHelper, WebHelper>();
            services.AddScoped<IFileStreamService, FileStreamService>();
            services.AddScoped<ILanguageService, LanguageService>();
            #endregion

            #region Mail sender
            services.AddScoped<IEmailSender, AuthMessageSender>();
            #endregion

            #region Others
            services.AddTransient<IPageBuilderService,PageBuilderService>();
            services.AddAQPagination();
            #endregion
        }
        #endregion
    }
}
