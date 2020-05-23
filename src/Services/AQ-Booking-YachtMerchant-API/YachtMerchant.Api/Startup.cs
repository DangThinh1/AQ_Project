using APIHelpers;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Core.Helpers;
using AQBooking.Core.Helpers;
using AQConfigurations.Core.Extensions;
using AutoMapper;
using DiningMerchant.Infrastructure.Services;
using Identity.Core.Extensions;
using Identity.Core.Helpers;
using Identity.Core.Services.Implements;
using Identity.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;
using YachtMerchant.Core.Helpers;
using YachtMerchant.Infrastructure.AppSetting;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Helpers;
using YachtMerchant.Infrastructure.Interfaces;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using YachtMerchant.Infrastructure.Mapper;
using YachtMerchant.Infrastructure.RequestServices.Implements;
using YachtMerchant.Infrastructure.RequestServices.Interfaces;
using YachtMerchant.Infrastructure.Services;
using YachtMerchant.Infrastructure.Services.YachtTour;

namespace YachtMerchant.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOption(Configuration);
            ApiHelper.Init(Configuration);
            WebHelper.Init(ApiHelper.GetServer());

            // Init & register Automapper
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<YachtOperatorDbContext>(options =>
            options.UseSqlServer(ApiUrlHelper.ConnectionString));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Yacht Merchant Api", Version = "v1" });
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

                // Configure Swagger to use the xml documentation file
                var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                c.IncludeXmlComments(xmlFile);
            });
            services.AddAuthentication();
            services.AddHttpContextAccessor();
            services.AddIdentityApiService(Configuration);
            services.AddDependencyInjection();
            // Add Configuration API service
            services.AddConfigurationsRequestService(ApiUrlHelper.ConfigurationApi);
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowMyOrigin"));
            });
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

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "YachtMerchant Api v1");
            });
            app.UseAQCors();
        }

       
    }

    public static class ServiceCollectionExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            #region Common Service

            services.TryAddScoped<IWorkContext, WorkContext>();
            services.TryAddScoped<IUsersContext, UsersContext>();
            services.TryAddScoped<ILocationRequestService, LocationRequestService>();
            services.TryAddScoped<ICheckHealthService, CheckHealthService>();

            #endregion Common Service

            #region Yacht Service

            services.TryAddScoped<IYachtMerchantService, YachtMerchantService>();
            services.TryAddScoped<IYachtOtherInformationService, YachtOtherInformationService>();
            services.TryAddScoped<IYachtInformationService, YachtInformationService>();
            services.TryAddScoped<IYachtNonBusinessDaysService, YachtNonBusinessDaysService>();
            services.TryAddScoped<IYachtAttributeService, YachtAttributeService>();
            services.TryAddScoped<IYachtAttributeValueService, YachtAttributeValueService>();
            services.TryAddScoped<IYachtPricingPlansService, YachtPricingPlansService>();
            services.TryAddScoped<IYachtService, YachtService>();
            services.TryAddScoped<IYachtCharteringService, YachtCharteringService>();
            services.TryAddScoped<IYachtFileStreamService, YachtFileStreamService>();
            services.TryAddScoped<IYachtMerchantProductVendorServices, YachtMerchantProductVendorServices>();
            services.TryAddScoped<IYachtMerchantProductInventoryService, YachtMerchantProductInventoryService>();
            services.TryAddScoped<IYachtPortService, YachtPortService>();
            services.TryAddScoped<IYachAdditionalService, YachAdditionalService>();
            services.TryAddScoped<IYachtMerchantInformationService, YachtMerchantInformationService>();
            services.TryAddScoped<IYachtCharteringSchedulesService, YachtCharteringSchedulesService>();
            services.TryAddScoped<IYachtCharteringPaymentLogsService, YachtCharteringPaymentLogsService>();
            services.TryAddScoped<IYachtCharteringProcessingFeesService, YachtCharteringProcessingFeesService>();
            services.TryAddScoped<IYachtMerchantCharterFeeOptionsService, YachtMerchantCharterFeeOptionsService>();
            services.TryAddScoped<IYachtMerchantUsersService, YachtMerchantUsersService>();
            services.TryAddScoped<IYachtCalendarService, YachtCalendarService>();
            services.TryAddScoped<IYachtMerchantProductSupplierServices, YachtMerchantProductSupplierServices>();
            services.TryAddScoped<IProductPricingServices, ProductPricingServices>();
            #endregion Yacht Service

            #region YachtTour Service
            services.TryAddScoped<IYachtTourCharterService, YachtTourCharterService>();
            services.TryAddScoped<IYachtTourCharterPaymentLogsService, YachtTourCharterPaymentLogsService>();
            services.TryAddScoped<IYachtTourCharterProcessingFeesService, YachtTourCharterProcessingFeesService>();
            services.TryAddScoped<IYachtTourCharterSchedulesService,YachtTourCharterSchedulesService>();
            services.TryAddScoped<IYachtToursServices, YachtTourServices>();
            services.TryAddScoped<IYachtTourCounterServices, YachtTourCounterServices>();
            services.TryAddScoped<IYachtTourFileStreamService, YachtTourFileStreamService>();
            services.TryAddScoped<IYachtTourNonOperationDayService, YachtTourNonOperationDayService>();
            services.TryAddScoped<IYachtTourExternalRefLinkServices, YachtTourExternalRefLinkServices>();
            services.TryAddScoped<IYachtTourOperationDetailService, YachtTourOperationDetailService>();
            services.TryAddScoped<IYachtTourInformationServices, YachtTourInformationServices>();
            services.TryAddScoped<IYachtTourAttributeService, YachtTourAttributeService>();
            services.TryAddScoped<IYachtTourCategoryService, YachtTourCategoryService>();
            services.TryAddScoped<IYachtTourAttributeValueService, YachtTourAttributeValueService>();
            services.TryAddScoped<IYachtTourPricingService, YachtTourPricingService>();

            #endregion YachtTour Service

            #region PreviewYachtTourDetail
            services.TryAddScoped<IPreviewYachtTourDetail, PreviewYachtTourDetail>();
            #endregion YachtTour Service
        }


        public static void AddOption(this IServiceCollection services, IConfiguration configuration)
        {
            DependencyInjectionHelper.Init(ref services);
            IdentityInjectionHelper.Init(ref services);
            services.AddOptions();
            services.Configure<ApiServer>(configuration.GetSection("ApiServer"));
        }
    }
}