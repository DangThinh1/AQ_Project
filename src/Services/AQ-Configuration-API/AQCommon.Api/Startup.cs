using System.IO;
using AutoMapper;
using Identity.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using AQConfigurations.Core.Extensions;
using Microsoft.Extensions.Configuration;
using AQConfigurations.Infrastructure.Helpers;
using AQConfigurations.Core.Models.AppSettings;
using Microsoft.Extensions.DependencyInjection;
using AQConfigurations.Infrastructure.Databases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AQConfigurations.Infrastructure.Services.Implements;
using AQConfigurations.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AQConfigurations.Core.Models.AppSettings.Helpers;
using AQConfigurations.Infrastructure.Models;

namespace AQConfigurations.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            HostingEnvironment = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Config AppSetting
            services.AddAppSettingBase()
                    .AddAppSettingConnectionString("AQConfigurations")
                    .AddAppSettingValue<YachtPortal>()
                    .WaitForExecute();

            //.................................................................
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "AQConfigurations Api", Version = "v1" });
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
            services.AddMemoryCache();
            services.AddAQCors();
            services.AddSevices();

            //Settup Database
            var connectionString = services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionString>>().Value.Value();
            services.AddDbContext<AQConfigurationsDbContext>(options => options.UseSqlServer(connectionString));
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

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "AQConfigurations Api v1");
            });
            app.UseAQCors();
        }
    }

    public static class ServiceCollectionExtension
    {
        public static void AddSevices(this IServiceCollection services)
        {
            services.TryAddScoped<ICurrencyService, CurrencyService>();
            services.TryAddScoped<ICommonValueService, CommonValueService>();
            services.TryAddScoped<IPortalLanguageService, PortalLanguageService>();
            services.TryAddScoped<ICommonResourceService, CommonResourceService>();
            services.TryAddScoped<ICommonLanguagesServices, CommonLanguagesServices>();
            services.TryAddScoped<IPortalLocationControlService, PortalLocationControlService>();
            services.TryAddScoped<ICitiesService, CitiesService>();
            services.TryAddScoped<IZoneDistrictService, ZoneDistrictService>();
            services.TryAddScoped<ICountriesService, CountriesService>();
        }
    }
}