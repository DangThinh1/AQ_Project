using Identity.Core.Common;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Identity.Core.Portal.Helpers
{
    public static class PersistentProviderHelper
    {
        public static IDataProtectionProvider CreateRedisProvider(IServiceCollection services,string host,string port)
        {
            var redisConnection = RedisCacheHelper.ConnectToRedisServer(host,port);
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
