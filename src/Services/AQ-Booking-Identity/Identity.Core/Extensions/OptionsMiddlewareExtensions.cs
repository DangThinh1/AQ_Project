using Identity.Core.Conts;
using Identity.Core.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Core.Extensions
{
    public static class OptionsMiddlewareExtensions
    {
        public static void AddAQCors(this IServiceCollection services, string scheme = AQCorsPolicy.DefaultScheme)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(scheme,
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials());
            });
        }

        public static IApplicationBuilder UseAQCors(this IApplicationBuilder builder, string scheme = AQCorsPolicy.DefaultScheme)
        {
            builder.UseCors(scheme);
            builder.UseOptions();
            return builder;
        }

        public static IApplicationBuilder UseOptions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OptionsMiddleware>();
        }
    }
}
