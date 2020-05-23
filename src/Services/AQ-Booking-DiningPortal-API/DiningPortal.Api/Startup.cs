using System;
using System.Collections.Generic;
using System.IO;
using AQBooking.Core.Helpers;
using AQConfigurations.Core.Extensions;
using AutoMapper;
using BookingPortal.Infrastructure.Services;
using AQDiningPortal.Infrastructure.Database;
using AQDiningPortal.Infrastructure.Helpers;
using AQDiningPortal.Infrastructure.Interfaces;
using AQDiningPortal.Infrastructure.Services;
using Identity.Core.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using AQDiningPortal.Infrastructure.AppSetting;

namespace AQDiningPortal.Api
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
            // Please don't move this line anywhere
            services.AddAppSetting(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Init & register Automapper
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "DiningPortal Api", Version = "v1" });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                   { JwtBearerDefaults.AuthenticationScheme, new string[] { } },
               });

                //// Configure Swagger to use the xml documentation file
                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
            });
            services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenHelper.GetValidationParameters();
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
            {
                o.Cookie.Name = ".AQB_D_P_A";
            });
            services.AddHttpContextAccessor();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            services.AddDependencyInjection();
            services.AddCustomDbContext();
            services.AddConfigurationsRequestService(ApiServerHelper.ConfigurationApi);
            services.AddAQCors();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "DingingPortal Api v1");
            });
            app.UseCookiePolicy();// Please don't move this line to anywhere
            app.UseAQCors();
        }
    }

    public static class ServiceCollectionExtension
    {
        public static void AddCustomDbContext(this IServiceCollection services)
        {
            string connectionString = ApiServerHelper.ConnectionString;
            services.AddDbContext<DiningSearchContext>(options =>
            options.UseSqlServer(connectionString));
            services.AddDbContext<DiningReservationContext>(options =>
            options.UseSqlServer(connectionString));
            services.AddDbContext<DinningVenueContext>(options =>
            options.UseSqlServer(connectionString));
        }

        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IRestaurantCounterService, RestaurantCounterService>();
            services.AddScoped<IRestaurantAttributeService, RestaurantAttributeService>();
            services.AddScoped<IRestaurantMerchantService, RestaurantMerchantService>();
            services.AddScoped<IRestaurantVenueService, RestaurantVenueService>();
        }
       
        public static void AddAppSetting(this IServiceCollection services, IConfiguration configuration)
        {
            DependencyInjectionHelper.Init(ref services);
            JwtTokenHelper.Init(configuration);
            services.AddOptions();
            services.Configure<ApiServer>(configuration.GetSection("ApiServer"));
        }
    }
}