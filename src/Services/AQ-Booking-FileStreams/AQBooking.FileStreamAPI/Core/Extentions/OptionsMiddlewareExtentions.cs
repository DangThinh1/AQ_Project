using AQBooking.FileStream.Core.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.FileStreamAPI.Core.Extentions
{
    public static class OptionsMiddlewareExtentions
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
            return builder.UseMiddleware<OptionMiddleware>();
        }
    }

    public class OptionMiddleware
    {
        private readonly RequestDelegate _next;

        public OptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            return BeginInvoke(context);
        }

        private Task BeginInvoke(HttpContext context)
        {
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { (string)context.Request.Headers["Origin"] });
                context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
                context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
                context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
                context.Response.StatusCode = 200;
                return context.Response.WriteAsync("OK");
            }

            return _next.Invoke(context);
        }
    }
}
