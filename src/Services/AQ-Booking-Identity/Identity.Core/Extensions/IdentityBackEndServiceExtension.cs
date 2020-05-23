using Identity.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Identity.Core.Models.JwtToken;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Identity.Core.Services.Interfaces;
using Identity.Core.Services.Implements;
using System;

namespace Identity.Core.Extensions
{
    public static  class IdentityBackEndServiceExtension
    {
        [Obsolete("Please use AddIdentityApiService instead")]
        public static void AddIdentityBackEndService(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityInjectionHelper.Init(ref services);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUsersContext, UsersContext>();
            services.AddOptions();
            services.Configure<JwtTokenOption>(configuration.GetSection("JwtTokenOption"));
            services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenBuilderHelper.DefaultBuilder.GetValidationParameters();
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
            });
        }
    }
}
