using System.Net;
using AutoMapper;
using Identity.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Swagger;
using Identity.Infrastructure.Helpers;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.AppSetting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Identity.Infrastructure.Database.Entities;
using Identity.Infrastructure.Services.Implements;
using Identity.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace AQBooking.Identity
{
    public class Startup
    {
        private int _server = 0;
        public IConfigurationRoot WebConfiguration { get; set; }
        private const string ALLOWED_USERNAME_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _server = Configuration.GetSection("WebSetting").GetValue<int>("Server");
            WebConfiguration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .Build();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            //*******************Custom Dependency*******************//
            services.AddStaticHelper();
            services.AddDependencyInjection();
            services.AddAppSetting(Configuration);
            services.AddIdentityBackEndService(Configuration);

            //***********************************************************************************//
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(ApiServerHelper.ConnectionString));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "AQ Admin Identity Api", Version = "v1" });
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
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
                    options.ValidationInterval = TimeSpan.FromMinutes(30));
            services.AddIdentity<Users, Roles>(options =>
            {
                options.User.AllowedUserNameCharacters = ALLOWED_USERNAME_CHARACTERS;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddHttpContextAccessor();
            // Init & register Automapper
            services.AddAutoMapper(typeof(AutoMapperProfile));
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "AQ Admin Identity v1");
            });
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseAuthentication();
            app.UseAQCors();
        }
    }

    public static class ServiceCollectionExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            //********************************************************************//
            services.TryAddScoped<IAccountService, AccountService>();
            services.TryAddScoped<IAuthenticateService, AuthenticateService>();
            services.TryAddScoped<IRoleService, RoleService>();
            services.TryAddScoped<ISignInService, SignInService>();
            services.TryAddScoped<IEmailService, EmailService>();
        }

        public static void AddStaticHelper(this IServiceCollection services)
        {
            DependencyInjectionHelper.Init(ref services);
        }

        public static void AddAppSetting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ApiServer>(configuration.GetSection("ApiServer"));
        }
    }
}
