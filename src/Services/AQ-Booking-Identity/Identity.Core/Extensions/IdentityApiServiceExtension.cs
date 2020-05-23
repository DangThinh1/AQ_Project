using Identity.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Identity.Core.Models.JwtToken;
using Identity.Core.Services.Interfaces;
using Identity.Core.Services.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Identity.Core.Extensions
{
    public static class IdentityApiServiceExtension
    {
        public static void AddIdentityApiService(this IServiceCollection services)
        {
            IdentityInjectionHelper.Init(ref services);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUsersContext, UsersContext>();
            services.AddJwtToken();
        }
        public static void AddIdentityApiService(this IServiceCollection services, JwtTokenOption jwtTokenOption)
        {
            IdentityInjectionHelper.Init(ref services);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUsersContext, UsersContext>();
            services.AddJwtToken(jwtTokenOption);
        }
        public static void AddIdentityApiService(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityInjectionHelper.Init(ref services);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUsersContext, UsersContext>();
            services.AddJwtToken(configuration);
        }

        public static void AddJwtToken(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenBuilderHelper.DefaultBuilder.GetValidationParameters();
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
            });
        }
        public static void AddJwtToken(this IServiceCollection services, JwtTokenOption jwtTokenOption)
        {
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new JwtTokenBuilder(jwtTokenOption).GetValidationParameters();
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
            });
        }
        public static void AddJwtToken(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<JwtTokenOption>(configuration.GetSection("JwtTokenOption"));
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenBuilderHelper.DefaultBuilder.GetValidationParameters();
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
            });
        }
    }
}