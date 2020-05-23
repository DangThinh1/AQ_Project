using AQBooking.YachtPortal.Infrastructure.AppSetting;
using Microsoft.Extensions.Options;
using System;
using static AQBooking.YachtPortal.Infrastructure.AppSetting.RedisCacheSrv;

namespace AQBooking.YachtPortal.Infrastructure.Helpers
{
    public class ApiServerHelper
    {
        public static ApiServer apiServer => DependencyInjectionHelper.GetService<IOptions<ApiServer>>().Value;

        public static int CurrentServer => apiServer.Server;


        public static RedisCache RedisCacheSrv
        {
            get
            {
                var server = apiServer.RedisCacheSrv;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        

        public static string ConfigurationApi
        {
            get
            {
                var server = apiServer.ConfigurationApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string ConnectionString
        {
            get
            {
                var server = apiServer.ConnectionString;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }
    }
}
