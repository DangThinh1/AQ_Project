using Identity.Core.Common;
using Identity.Core.Portal.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.Core.Portal.Extensions
{
    public static class PersistentProviderExtension
    {
        public static void AddDataProtectRedisProvider(this IServiceCollection services,string host,string port)
        {
            var redisConnection = RedisCacheHelper.ConnectToRedisServer(host,port);
            services.AddSingleton(options => redisConnection.GetDatabase());
            services.AddDataProtection()
                .SetApplicationName(SecurityConstant.AQSecurityMasterAppName)
                .PersistKeysToStackExchangeRedis(redisConnection,SecurityConstant.AQSecurityMasterKeyRedis)
                .SetDefaultKeyLifetime(TimeSpan.FromDays(365 * 10));
        }
    }
}
