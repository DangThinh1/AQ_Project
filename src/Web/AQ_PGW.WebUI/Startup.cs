using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Infrastructure.DataContext;
using AQ_PGW.Infrastructure.Repositories;
using AQ_PGW.Infrastructure.Servives;
using AQ_PGW.WebUI.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace AQ_PGW.WebUI
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

            var host = "";
            switch (Configuration["ApiHost_ServerEnvironment"])
            {
                case "Localhost":
                    host = "http://localhost:302/";
                    break;
                case "VN":
                    host = "http://172.16.10.138/aq-pgw-api/";
                    break;
                default:
                    host = "http://localhost:302/";
                    break;
            }
            Host.HostName = host;
            var server = Configuration["Server:DB_ServerEnvironment"];

            string connectionString = Configuration.GetConnectionString("AppDbConnection" + server);
            services.AddDbContext<AQ_PaymentGatewayDBContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddScoped<Infrastructure.Repositories.IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransactionsServiceRepository, TransactionsServiceRepository>();
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
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                     defaults: new { controller = "Home", action = "Index" }
                     );

            });
        }
    }
}
