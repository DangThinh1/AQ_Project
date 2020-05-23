using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQBooking.YachtPortal.Infrastructure.Mapping;
using AQBooking.YachtPortal.Infrastructure.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Identity.Core.Extensions;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using AQBooking.YachtPortal.Infrastructure.AppSetting;
using AQBooking.Core.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AQBooking.YachtPortal.API
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "AQ YACHT API",
                    Version = "v1.0",
                    Description = "API document for module AQ yacht",
                    TermsOfService = "None",
                });
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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();
            });

            // Init & register Automapper
            services.AddAutoMapper(typeof(AutoMapperConfig));
            services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenHelper.GetValidationParameters();
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
            {
                o.Cookie.Name = ".AQB_Y_P_A";
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            services.AddAQCors();
            services.AddCustomizedDbContext();
            services.AddDependencyInjection();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "AQ Yacht Api");
            });
            app.UseCookiePolicy();//************Please don't move this line to anywhere************
            app.UseAQCors();
        }
    }

    public static class ServiceCollectionExtension
    {
        public static void AddAppSetting(this IServiceCollection services, IConfiguration configuration)
        {
            DependencyInjectionHelper.Init(ref services);
            JwtTokenHelper.Init(configuration);
            services.AddOptions();
            services.Configure<ApiServer>(configuration.GetSection("ApiServer"));
        }

        public static void AddCustomizedDbContext(this IServiceCollection services)
        {
            string connectionString = ApiServerHelper.ConnectionString;
            services.AddDbContext<AQYachtContext>(options => options.UseSqlServer(connectionString));

            /**Microsoft.Extensions.Caching.Redis 
             *Start Redis Caching
            **/

            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = ApiServerHelper.RedisCacheSrv.InstanceName;
                options.Configuration = $"{ApiServerHelper.RedisCacheSrv.Host}:{ApiServerHelper.RedisCacheSrv.Port}";
            });
            /**Microsoft.Extensions.Caching.Redis 
             * END Redis Caching
            **/
        }

        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IYachtMerchantService, YachtMerchantService>();
            services.AddScoped<IPortLocationService, PortLocationService>();
            services.AddScoped<IYachtMerchantService, YachtMerchantService>();
            services.AddScoped<IYachtAttributevalueService, YachtAttributeValueService>();
            services.AddScoped<IYachtFileStreamService, YachtFileStreamService>();
            services.AddScoped<IYachtPricingPlanInfomationService, YachtPricingPlanInfomationService>();
            services.AddScoped<IYachtAdditionalService, YachtAdditionalService>();
            services.AddScoped<IYachtMerchantProductInventoryService, YachtMerchantProductInventoryService>();
            services.AddScoped<IYachtInformationDetailService, YachtInformationDetailService>();
            services.AddScoped<IYachtPricingPlanDetailService, YachtPricingPlanDetailService>();
            services.AddScoped<IYachtCharteringService, YachtCharteringService>();
            services.AddScoped<IYachtCharteringPaymentLogService, YachtCharteringPaymentLogService>();
            services.AddScoped<IYachtService, YachtService>();
            services.AddScoped<IYachtTourService, YachtTourService>();
            services.AddScoped<IYachtTourCategoryService, YachtTourCategoryService>();
            services.AddScoped<IYachtLandingService, YachtLandingService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();
        }
    }




}
