using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Helpers;
using AQBooking.Admin.Infrastructure;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AQBooking.Admin.Infrastructure.Services;
using AQBooking.Admin.Infrastructure.Databases.ConfigEntities;
using AQBooking.Admin.Infrastructure.Databases.DiningEntities;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
using AQBooking.Admin.Infrastructure.Databases.EvisaEntities;
using AQBooking.Admin.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using AQBooking.Admin.Infrastructure.Databases.HotelEntities;
using AQBooking.Admin.API.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AQBooking.Admin.Infrastructure.Databases.CMSEntities;

namespace AQBooking.Admin.API
{
    public class Startup
    {
        public IConfigurationRoot WebConfiguration { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebConfiguration = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json")
           .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ApiHelper.Init(Configuration);
            WebHelper.Init(ApiHelper.GetServer());
            JwtTokenHelper.Init(Configuration);

            //services.AddMvc(options => {
            //    options.Filters.Add(typeof(ExceptionFilter));
            //    options.OutputFormatters.Insert(0, new CustomOutputFormater());
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<AQConfigContext>(options =>
                options.UseSqlServer(ApiHelper.GetConnectionStringConfig()));

            services.AddDbContext<AQYachtContext>(options =>
                options.UseSqlServer(ApiHelper.GetConnectionStringYacht()));

            services.AddDbContext<AQDiningContext>(options =>
                options.UseSqlServer(ApiHelper.GetConnectionStringDining()));

            services.AddDbContext<AQEvisaContext>(options =>
                options.UseSqlServer(ApiHelper.GetConnectionStringEvisa()));

            services.AddDbContext<AQHotelContext>(options =>
                options.UseSqlServer(ApiHelper.GetConnectionStringHotel()));

            services.AddDbContext<AQCMSContext>(options =>
                options.UseSqlServer(ApiHelper.GetConnectionStringCMS()));

            ServiceHelper.Init(services.BuildServiceProvider());

            services.AddSwaggerGen(c =>
            {

                //.Netcore 2.2
                /*c.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin Api", Version = "v1" });
                
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
                c.DescribeAllEnumsAsStrings();*/


                //Netcore 3.1
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
                
            });
            services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenHelper.GetValidationParameters();
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
            });

            // Init & register Automapper
            services.AddAutoMapper(typeof(AutoMapperConfig));
            services.AddHttpContextAccessor();
            RegisterDependencyInjection(services);
            EngineerContext.Init(services.BuildServiceProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "AQ Admin Api");
                //c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<IRestaurantMerchantService, RestaurantMerchantService>();
            services.AddScoped<IRestaurantMerchantAccService, RestaurantMerchantAccService>();
            services.AddScoped<IRestaurantMerchantAccMgtService, RestaurantMerchantAccMgtService>();
            services.AddScoped<IYachtMerchantAccService, YachtMerchantAccService>();
            services.AddScoped<IYachtMerchantAccMgtService, YachtMerchantAccMgtService>();
            services.AddScoped<IYachtMerchantService, YachtMerchantService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPortalLocationService, PortalLocationService>();
            services.AddScoped<IYachtMerchantCharterFeeService, YachtMerchantCharterFeeService>();
            services.AddScoped<IYachtMerchantFileStreamService, YachtMerchantFileStreamService>();
            services.AddScoped<IMembershipPrivilegeService, MembershipPrivilegeService>();
            services.AddScoped<IYachtAttributeService, YachtAttributeService>();
            services.AddScoped<IYachtTourAttributeService, YachtTourAttributeService>();
            services.AddScoped<IYachtTourCategoryService, YachtTourCategoryService>();
            services.AddScoped<ICommonValueService, CommonValueService>();
            services.AddScoped<ICommonResourceService, CommonResourceService>();
            services.AddScoped<ICommonLanguagueService, CommonLanguagueService>();
            services.AddScoped<IEVisaMerchantService, EVisaMerchantService>();
            services.AddScoped<IEVisaMerchantAccService, EVisaMerchantAccService>();
            services.AddScoped<IHotelMerchantService, HotelMerchantService>();
            services.AddScoped<IHotelMerchantUserService, HotelMerchantUserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostCategoryService, PostCategoryService>();
            services.AddScoped<ISubscriberService, SubscriberService>();
            services.AddScoped<IPostFileStreamService, PostFileStreamService>();
            services.AddScoped<IPostDetailService, PostDetailService>();
        }
    }
}