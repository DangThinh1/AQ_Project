using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Infrastructure.Repositories;
using AQ_PGW.Infrastructure.Servives;
using AQ_PGW.Infrastructure.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AQ_PGW.API.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using AutoMapper;
using AQ_PGW.API.MappingModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using PaypalExpressCheckout.BusinessLogic.Interfaces;
using PaypalExpressCheckout.BusinessLogic;
using PaypalExpressCheckout.BusinessLogic.ConfigOptions;

namespace AQ_PGW.API
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
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var signingKey = Convert.FromBase64String(Configuration["Jwt:SigningSecret"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                };
            });

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(RequestResponseFilterAsync)); // by type
            //});


            //services.AddMvc(o =>
            //{
            //    o.AllowEmptyInputInBodyModelBinding = true;
            //});

            //services.AddMvc().AddMvcOptions(options =>
            //    options.ModelMetadataDetailsProviders.Add(
            //    new ExcludeBindingMetadataProvider(typeof(System.Version))));

            //services.AddMvc().AddMvcOptions(options =>
            //    options.ModelMetadataDetailsProviders.Add(
            //    new SuppressChildValidationMetadataProvider(typeof(System.Guid))));
            var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Core API",
                    Description = "Swagger Core API"
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    In = "header",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
            var host = "";
            switch (Configuration["Host_ServerEnvironment"])
            {
                case "Localhost":
                    host = "http://localhost:302";
                    break;
                case "VN":
                    host = "http://172.16.10.138/aq-pgw-api/";
                    break;
                //case "LIVE":
                //    host = "http://172.16.10.138/aq-pgw-api/";
                //    break;
                default:
                    host = "http://localhost:302";
                    break;
            }
            Host.HostName = host;

            var server = Configuration["Server:DB_ServerEnvironment"];
            string connectionString = Configuration.GetConnectionString("AppDbConnection" + server);
            services.AddDbContext<AQ_PaymentGatewayDBContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<Infrastructure.Repositories.IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITransactionsServiceRepository, TransactionsServiceRepository>();
            services.AddScoped<ILoginServiceRepository, LoginServiceRepository>();
            services.AddScoped<IPaymentLogsServiceRepository, PaymentLogsServiceRepository>();
            services.AddScoped<ISystemLogsServiceRepository, SystemLogsServiceRepository>();
            services.AddScoped<ILinksRelatedServiceRepository, LinksRelatedServiceRepository>();
            services.AddScoped<IRelatedTransactionDetailsServiceRepository, RelatedTransactionDetailsServiceRepository>();
            services.AddScoped<IRefundPaypalServiceRepository, RefundPaypalServiceRepository>();
            services.AddScoped<IRefundStripeServiceRepository, RefundStripeServiceRepository>();

            services.AddSingleton<IPaypalServices, PaypalServices>();
            services.Configure<PayPalAuthOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //https://docs.microsoft.com/vi-vn/aspnet/core/test/troubleshoot-azure-iis?view=aspnetcore-2.2
            //config IIS
            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //localhost
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API");
                });
            }
            else
            {
                if (Configuration["Host_ServerEnvironment"] == "LIVE")
                {
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API");
                    });
                }
                else
                {
                    //VN
                    app.UseSwaggerUI(c =>
                    {
                        string swaggerJsonBasePath = Host.HostName;// string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                        c.SwaggerEndpoint(swaggerJsonBasePath + "/swagger/v1/swagger.json", "Core API");
                        c.RoutePrefix = string.Empty;
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core API");

                    });
                }
               
            }
            //app.UseMiddleware<RequestResponseLoggingMiddleware>();

            //app.UseMvc();
            app.UseAuthentication();
            app.UseCors(builder =>
                         builder.WithOrigins("*")
                         .AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Transaction}/{action=GetPayment}/{id?}");
            });




            //host VN
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/aq-pgw-api/swagger/v1/swagger.json", "Core API");
            //});
        }
    }
}
