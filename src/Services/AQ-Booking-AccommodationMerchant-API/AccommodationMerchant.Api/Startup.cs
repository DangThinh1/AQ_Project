using System.IO;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AQConfigurations.Core.Extensions;
using AQConfigurations.Core.Models.AppSettings;
using AccommodationMerchant.Infrastructure.Mappers;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using AccommodationMerchant.Infrastructure.Services.Implements;
using AccommodationMerchant.Api.Attributes;
using Identity.Core.Extensions;
using Identity.Core.Models.JwtToken;
using Swashbuckle.AspNetCore.Swagger;
using APIHelpers;
using AutoMapper;
using AccommodationMerchant.Core.Helpers;
using AQConfigurations.Core.Services.Interfaces;
using AQConfigurations.Core.Services.Implements;

namespace AccommodationMerchant.Api
{
    //kietcomment
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to a dd services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Setup AppSetting (must be on top)
            services.AddAppSettingBase()
                    .AddAppSettingConnectionString("AQAccommodation")
                    .AddAppSettingValue<ConfigurationsApi>()
                    .AddAppSettingValue<JwtTokenOption>()
                    .AddAppSettingValue<ConfigurationsApi>()
                    .WaitForExecute();
            //Setup Basic
            services.AddMvc(options => {
                options.OutputFormatters.Add(new AQOutputFormatter());
                options.Filters.Add(typeof(ExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Accommodation Merchant Api", Version = "v1" });
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
            //Register domain services
            services.AddServices();

            //Setup Database
            var connectionString = services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionString>>().Value.Value();
            services.AddDbContext<AccommodationContext>(options => options.UseSqlServer(connectionString));

            //Setup Identity
            var jwtTokenOption = services.BuildServiceProvider().GetRequiredService<IOptions<JwtTokenOption>>().Value;
            services.AddIdentityApiService(jwtTokenOption);

            //Setup ConfigurationsApi 
            var configurationsApi = services.BuildServiceProvider().GetRequiredService<IOptions<ConfigurationsApi>>().Value.Value();
            services.AddConfigurationsRequestService(configurationsApi);
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Accommodation Merchant Api v1");
            });

            app.UseMvc();
        }
    }
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            DependencyInjectionHelper.TryInit(ref services);
            services.TryAddScoped<IHotelAttributeService, HotelAttributeService>();
            services.TryAddScoped<IHotelAttributeValueService, HotelAttributeValueService>();
            services.TryAddScoped<IHotelMerchantService,HotelMerchantService>();
            services.TryAddScoped<IHotelMerchantUserService, HotelMerchantUserService>();
            services.TryAddScoped<IHotelReservationPaymentLogsService, HotelReservationPaymentLogsService>();
            services.TryAddScoped<IHotelReservationProcessingFeesService, HotelReservationProcessingFeesService>();
            services.TryAddScoped<IHotelReservationsService, HotelReservationsService>();
            services.TryAddScoped<IHotelService, HotelService>();
            services.TryAddScoped<IHotelInformationDetailService, HotelInformationDetailService>();
            services.TryAddScoped<IHotelFileStreamService, HotelFileStreamService>();
            services.TryAddScoped<IHotelInventoryFileStreamService, HotelInventoryFileStreamService>();
            services.TryAddScoped<IHotelInformationService, HotelInformationService>();
            services.TryAddScoped<IHotelInventoryService, HotelInventoryService>();
            services.TryAddScoped<IHotelInventoryAttributeService, HotelInventoryAttributeService>();
            services.TryAddScoped<IHotelInventoryAttributeValueService, HotelInventoryAttributeValueService>();
            services.TryAddScoped<ICommonLanguagesRequestServices, CommonLanguagesRequestServices>();
        }
    }
}