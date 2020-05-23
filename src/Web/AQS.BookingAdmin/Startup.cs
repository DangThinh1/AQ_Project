using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Interfaces.User;
using AQS.BookingMVC.Services;
using AQS.BookingAdmin.Services.Implements.User;
using AQS.BookingAdmin.Services.Interfaces.User;

using AQS.BookingAdmin.Services.Implements.Common;
using AQS.BookingAdmin.Services.Interfaces.Posts;
using AQS.BookingAdmin.Services.Implements.Posts;
using AQS.BookingAdmin.Services.Interfaces.Common;

namespace AQS.BookingAdmin
{
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
            services.AddSessionAQS();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddAQSAuthentication();
            ConfigureExternalServices(services);
            ConfigureAQSServices(services);
            services.AddAppSettings(Configuration);


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
                //  app.UseDatabaseErrorPage();
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

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });


        }

        #endregion

        #region Config External Services
        private void ConfigureExternalServices(IServiceCollection services)
        {
            //Register Nuget services here
            //configuation request service
            services.AddAQConfigurationsService(Configuration);
        }
        #endregion

        #region Config AQS Services
        private void ConfigureAQSServices(IServiceCollection services)
        {
            //Register app service here
            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFileStreamService, FileStreamService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommonValueService, CommonValueService>();
            services.AddScoped<ICommonLanguageService, CommonLanguageService>();
            services.AddScoped<IPostCategoryService, PostCategoryService>();
            services.AddScoped<IPostCategoryDetailService, PostCategoryDetailService>();
            services.AddScoped<ISelectListService, SelectListService>();
            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSubcriberService, EmailSubcriberService>();
            services.AddScoped<IAQFileProvider, AQFileProvider>();
        }
        #endregion



    }
}
