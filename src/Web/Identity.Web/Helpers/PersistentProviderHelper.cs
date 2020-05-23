using System;
using Identity.Core.Common;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Identity.Core.Portal.Helpers;
using Identity.Web.AppConfig;

namespace Identity.Web.Helpers
{
    public static class PersistentProviderHelper
    {
        public static IDataProtectionProvider CreateRedisProvider(IServiceCollection services)
        {
            var redisConnection = RedisCacheHelper.ConnectToRedisServer(ApiUrlHelper.RedisCacheSrv.Host, ApiUrlHelper.RedisCacheSrv.Port);
            services.AddSingleton(options => redisConnection.GetDatabase());
            return new ServiceCollection()
                .AddDataProtection()
                .SetApplicationName(SecurityConstant.AQSecurityMasterAppName)
                .PersistKeysToStackExchangeRedis(redisConnection, SecurityConstant.AQSecurityMasterKeyRedis)
                .SetDefaultKeyLifetime(TimeSpan.FromDays(365 * 10))
                .Services
                .BuildServiceProvider()
                .GetRequiredService<IDataProtectionProvider>();
        }
    }
}
