using AQBooking.Core;
using AQBooking.FileStream.Core.Helpers;
using AQBooking.FileStream.Infrastructure.Entities;
using AQBooking.FileStream.Infrastructure.Helps;
using AQBooking.FileStreamWeb.AppConfig;
using AQBooking.FileStreamWeb.Helpers.AQPagination;
using AQBooking.FileStreamWeb.Interfaces;
using AQBooking.FileStreamWeb.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AQBooking.FileStreamWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IFileManagementService, FileManagementService>();
            services.AddScoped<IPaginatedService, PaginatedService>();
            services.AddStaticHelper();
            services.AddOption(Configuration);
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
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public static class ServiceCollectionExtension
    {
        public static void AddStaticHelper(this IServiceCollection services)
        {
            DependencyInjectionHelper.Init(services);
            EngineerContext.Init(services.BuildServiceProvider());
        }
        public static void AddOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ServerConfig>(configuration.GetSection("ServerConfig"));
            services.Configure<ApiUrlConfig>(configuration.GetSection("ApiUrlConfig"));
        }
    }
}
