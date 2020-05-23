using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using AQBooking.FileStreamAPI.Core.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using AQBooking.FileStream.Core.Models.Configuration;
using AQBooking.FileStream.Core.Models.Media;
using AQBooking.FileStream.Infrastructure.Interfaces;
using AQBooking.FileStream.Infrastructure.Services;
using AQBooking.FileStream.Infrastructure.Helps;
using AQBooking.FileStream.Infrastructure.Entities;
using APIHelpers;
using AQBooking.FileStream.Core.Constants;
using AQBooking.FileStreamAPI.Core.Extentions;

namespace AQBooking.FileStreamAPI
{
    public class Startup
    {
        int server;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AQCorsPolicy.DefaultScheme,
                builder =>
                {
                    builder.AllowAnyOrigin().SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.AddMvc(options =>
            {
                //custom formater
                options.OutputFormatters.Insert(0, new CustomOutputFormater());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // Register the swagger generator, definding 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "AQ Booking HandleFiles",
                    Version = "v1.0",
                    Description = "Module handle files for project AQBooking",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "AQ Booking Teams",
                        Email = "aqteams@aqbooking.com"
                        
            }
                });

                c.AddSecurityDefinition("Basic", new BasicAuthScheme
                {
                    Type = "basic",
                    Description = "Input User Name And Password"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Basic", new string[] { } }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();
            });

            services.AddOptions();
            AddCustomizedDataStore(services, Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HostingConfig>();
            services.AddSingleton<MediaSettings>();
            services.AddTransient<IFileHandleService, FileHandleService>();
            services.AddTransient<IPictureService, PictureService>();
            services.AddTransient<IWebHelper, WebHelper>();
            services.AddTransient<IAQFileProvider, AQFileProvider>();
            services.AddScoped<IFileManagementService, FileManagementService>();
        }

        private IServiceCollection AddCustomizedDataStore(IServiceCollection services, IConfiguration configuration)
        {
            server = configuration.GetSection("ConnectionStrings").GetValue<int>("Server");

            var connection = configuration.GetConnectionString(ConfigUrlHelper.GetConnectionName(server));
            services.AddDbContext<AQ_FileStreamsContext>(options => options.UseSqlServer(connection));
            return services;
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
            app.Use(async (context, next) =>
            {
                context.Request.EnableRewind();
                await next();
            });
            app.UseHttpsRedirection();
            app.UseHttpContext();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
                // c.RoutePrefix = string.Empty;
            });

            app.UseCors(AQCorsPolicy.DefaultScheme);
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
